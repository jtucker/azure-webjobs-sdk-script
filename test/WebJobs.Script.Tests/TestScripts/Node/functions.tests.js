﻿var expect = require('chai').expect;

var config = process.argv[process.argv.length - 1]
config = (config.indexOf('--config=') !== -1)? config.substr(9): 'Debug';

function testRequire(script) {
    return require('../../bin/' + config + '/Content/Script/' + script);
}

var functions = testRequire('functions.js');
var response = testRequire('http/response.js');
var request = testRequire('http/request.js');

describe('http', () => {
    describe('response', () => {
        var res, context;
        beforeEach(() => {
            context = {
                isDone: false,
                done: () => context.isDone = true
            };
            res = response(context);
        });

        it('status', () => {
            res.status(200);
            expect(res.statusCode).to.equal(200);
            expect(context.isDone).to.be.false;
        });

        it('sendStatus', () => {
            res.sendStatus(204);
            expect(res.statusCode).to.equal(204);
            expect(context.isDone).to.be.true;
        });

        it('end', () => {
            res.end('test');
            expect(res.body).to.equal('test');
            expect(context.isDone).to.be.true;
        });

        it('send', () => {
            res.send('test');
            expect(res.body).to.equal('test');
            expect(context.isDone).to.be.true;
        });

        it('json', () => {
            res.json('test');
            expect(res.body).to.equal('test');
            expect(res.get('Content-Type')).to.equal('application/json');
            expect(context.isDone).to.be.true;
        });

        it('set', () => {
            res.set('header', 'val');
            expect(res.headers.header).to.equal('val');
            expect(context.isDone).to.be.false;
        });

        it('header', () => {
            res.header('header', 'val');
            expect(res.headers.header).to.equal('val');
            expect(context.isDone).to.be.false;
        });

        it('type', () => {
            res.type('text/html');
            expect(res.get('Content-Type')).to.equal('text/html');
            expect(context.isDone).to.be.false;
        });

        it('get', () => {
            res.set('header', 'val');
            expect(res.get('header')).to.equal('val');
            expect(context.isDone).to.be.false;
        });
    });

    describe('request', () => {
        var req, context;

        beforeEach(() => {
            context = {
                req: {
                    headers: { test: 'val' }
                }
            };

            req = request(context);
        });

        it('get', () => {
            expect(req.get('test')).to.equal('val');
        })
    });    
});

describe('functions', () => {
    var context = {};
    var logs = [];
    beforeEach(() => {
        logs = [];
        context = {
            _inputs: [],
            bindings: {},
            log: (message) => logs.push(message),
            bind: (val, cb) => cb && cb(val)
        };
    });

    it('clears require cache', (done) => {
        expect(Object.keys(require.cache).length).to.not.equal(0);

        functions.clearRequireCache(undefined, () => {
            expect(Object.keys(require.cache).length).to.equal(0);
            done();
        });
    });

    describe('entry point', () => {
        it('runs single export', () => {
            var run = false;
            var func = functions.createFunction({
                f: () => run = true,
            });

            func(context);

            expect(run).to.be.true;
        });

        it('runs named entry', () => {
            var run = false;
            var func = functions.createFunction({
                named: () => run = true,
                other: () => run = false
            });

            context._entryPoint = 'named';
            func(context);

            expect(run).to.be.true;
        });

        it('falls back to run function', () => {
            var run = false;
            var func = functions.createFunction({
                run: () => run = true,
                other: () => run = false
            });

            func(context);

            expect(run).to.be.true;
        });

        it('falls back to index function', () => {
            var run = false;
            var func = functions.createFunction({
                index: () => run = true,
                other: () => run = false
            });

            func(context);

            expect(run).to.be.true;
        });

        it('throws if not a function', () => {
            var func = functions.createFunction(1);
            expect(() => func(context)).to.throw(/Unable to determine function entry point.*/);
        });

        it('throws if object does not contain a function', () => {
            var func = functions.createFunction({ run: 1 });
            expect(() => func(context)).to.throw(/Unable to determine function entry point.*/);
        });
    });

    describe('wrapper', () => {
        it('logs if double done', () => {
            var func = functions.createFunction((context) => {
                context.done();
                context.done();
                expect(logs[0]).to.match(/Error: 'done' has already been called.*/);
            });

            func(context, () => {});
            
        });

        it('logs if promise and done', (done) => {
            var func = functions.createFunction((context) => {
                context.done();
                return Promise.resolve('test');
            });

            func(context, () => {
                setImmediate(() => {
                    expect(logs[0]).to.match(/Error: Choose either to return a promise or call 'done'.*/);
                    done();
                });
            });
        });

        it('done passes data to binder', () => {
            var func = functions.createFunction((context) => {
                context.bindings = { result: 'res' };
                context.done();
            });

            func(context, (results) => {
                expect(results).to.eql({ result: 'res' });
            });
        });

        it('done passes error', () => {
            var func = functions.createFunction((context) => {
                context.done('err');
            });

            func(context, (results) => {
                expect(results).to.eql('err');
            });
        });

        it('promise passes error', (done) => {
            var func = functions.createFunction((context) => {
                return Promise.reject('err');
            });

            func(context, (results) => {
                expect(results).to.eql('err');
                done();
            });
        });

        it('attaches context.res and context.req', () => {
            var func = functions.createFunction((context) => {
                context.res.status(200)
                    .header('header', context.req.get('field'))
                    .send('test');
            });

            context._triggerType = "httpTrigger";
            context.req = {
                headers: { 'field': 'val' }
            };
            func(context, (results) => {
                expect(context.res.statusCode).to.equal(200);
                expect(context.res.body).to.equal('test');
                expect(context.res.headers.header).to.equal('val');
                expect(context._http).to.be.undefined;
                expect(context._done).to.be.true;
            });
        });
    });
});