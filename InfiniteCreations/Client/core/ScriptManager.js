define([], function () {

    return class ScriptManager {

        constructor(pc) {
            this.pc = pc;
            this.path = 'core/scripts/';
            this.extension = '.js'
        }

        loadScript(a) {
            if (typeof a != 'string') return false;
            return new Promise((res, rej) => {
                let req = this.request(this.path + a + this.extension);
                req.send();
                let interval = setInterval(function () {
                    if (req.status === 200) {
                        clearInterval(interval);
                        return res(true)
                    } else if (req.status != 0) {
                        clearInterval(interval);
                        return res(false)
                    }
                }, 500);
            })

        }

        loadScripts(a) {
            if (typeof a != 'object') return false;
            return new Promise((res, rej) => {

                let scripts = [];

                for (var i in a) {
                    scripts.push(this.request(this.path + a[i] + this.extension));
                    scripts[scripts.length - 1].send();
                }

                let timeout = 0

                let interval = setInterval(function () {
                    for (var i in scripts) {
                        if (scripts[i].status === 200) {
                            scripts.splice(i, 1)
                        } else if (scripts[i].status != 0) {
                            scripts.splice(i, 1);
                        }
                    }

                    if (scripts.length === 0) {
                        clearInterval(interval);
                        return res(true);
                    } else if (timeout > 19) {
                        clearInterval(interval);
                        return res(false);
                    }


                    timeout++;
                }, 500);

            })
        }

        request(url) {
            let xhr = new XMLHttpRequest();
            xhr.addEventListener("load", function (resp) {
                if (xhr.status === 200) {
                    (new Function(['pc'], resp.target.response))(this.pc);
                } else {

                }
            }.bind(this));
            xhr.open('GET', url);
            return xhr;
        }

    }

})