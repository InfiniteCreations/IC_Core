define([], function () {

    class Event {

        constructor() {
            this.events = {};
        }

        on(name, func) {
            if (!this.events[name]) {
                this.events[name] = [];
            }
            this.events[name].push(func);
        }

        call(name, args) {
            if (this.events[name]) {
                for (var i = 0; i < this.events[name].length; i++) {
                    this.events[name][i].apply(null, args);
                }
            }
        }

    }

    return class EventManager {

        static registerEvent(name) {
            return new Event();
        }

    }




})