var Controller = pc.createScript('controller');

Controller.prototype.initialize = function () {
    this.playerSpeed = 1.385;
    this.jumpVelocity = 500.5;

    this.animations = {
        /*
        example: {
            name: 'walk.json',
            reverse: false,
            loop: true,
            blend: 0.2,
        }
        */
    };

    this.state = {
        jumping: false,
        walking: false,
        running: false,
        crouch: false,
        prone: false,
        flying: false,
        falling: false,
        invisible: false,
        invinsible: false,
    }

    this.app.keyboard.on(pc.EVENT_KEYDOWN, function (e) { }, this);
    this.app.keyboard.on(pc.EVENT_KEYUP, function (e) { }, this);
    this.app.keyboard.on(pc.EVENT_MOUSEDOWN, function (e) { }, this);
    this.app.keyboard.on(pc.EVENT_MOUSEMOVE, function (e) { }, this);

}


Controller.prototype.postInitialize = function () {

}

Controller.prototype.update = function (dT) {

}

Controller.prototype.postUpdate = function () {

}

Controller.prototype.swap = function () {

}
