var Camera = pc.createScript('camera');


Camera.prototype.initialize = function () {
    this.entity.addComponent('camera', {
        clearColor: new pc.Color(0.1, 0.1, 0.1)
    })
    this.entity.setPosition(0.08583033829927444, 0.5615254044532776, 1.1752979755401611);

    this.rotationSpeed = 0.5;
    this.cameraSpeed = 0.01;

    let eulers = this.entity.getLocalEulerAngles();

    this.ex = eulers.x;
    this.ey = eulers.y;

    // this.ey -= 180;
 
    
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
        this.ex -= e.dy / 5;
        this.ex = pc.math.clamp(this.ex, -90, 90);
        this.ey -= e.dx / 5;
    }


}

Camera.prototype.update = function () {

    this.entity.setLocalEulerAngles(this.ex, this.ey, 0);

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
