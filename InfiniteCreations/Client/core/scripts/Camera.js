var Camera = pc.createScript('camera');

Camera.prototype.initialize = function () {
    this.entity.addComponent('camera', {
        clearColor: new pc.Color(0.1, 0.1, 0.1)
    })
    this.entity.setPosition(0, 0, 5);

    this.rotationSpeed = 0.5;
    this.cameraSpeed = 0.2;

    /*
    var eulers = this.entity.getLocalEulerAngles();
    this.ex = eulers.x;
    this.ey = eulers.y;
    */

    this.eulers = new pc.Vec3();
    this.eulers.y = 90;
    
    this.app.mouse.on(pc.EVENT_MOUSEMOVE, this.onMouseMove, this);
    this.app.mouse.on(pc.EVENT_MOUSEDOWN, this.onMouseDown, this);
}

Camera.prototype.postInitialize = function () {

}

Camera.prototype.onMouseDown = function (e) {

    if (e.button === 0) {
        if (!pc.Mouse.isPointerLocked()) {
            this.app.mouse.enablePointerLock();
        }
    }
}


Camera.prototype.onMouseMove = function (e) {

    if (pc.Mouse.isPointerLocked()) {
        this.eulers.x -= this.rotationSpeed * (e.dx / 5);
        this.eulers.y -= this.rotationSpeed * (e.dy / 5);
        /*
        this.ex -= e.dy / 5;
        this.ex = pc.math.clamp(this.ex, 0, 180);
        if (e.dx > 0) {
            this.entity.rotate(0, 0, -this.rotationSpeed)
        } else {
            this.entity.rotate(0, 0, this.rotationSpeed)
        }
        */
    }


}

Camera.prototype.update = function () {

    this.entity.setLocalEulerAngles(this.eulers.y, 0, this.eulers.x);

    if (this.app.keyboard.isPressed(pc.KEY_W)) {
        this.entity.translateLocal(0, 0, -this.cameraSpeed)
    }

    if (this.app.keyboard.isPressed(pc.KEY_S)) {
        this.entity.translateLocal(0, 0, this.cameraSpeed);
    }

    if (this.app.keyboard.isPressed(pc.KEY_A)) {
        this.entity.translateLocal(-this.cameraSpeed, 0, 0)
    }

    if (this.app.keyboard.isPressed(pc.KEY_D)) {
        this.entity.translateLocal(this.cameraSpeed, 0, 0)
    }

}

Camera.prototype.postUpdate = function () {

}

Camera.prototype.swap = function () {



}
