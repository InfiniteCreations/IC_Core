define(['pc', 'Core/ScriptManager'], function (pc, ScriptManager) {

    return class InfiniteCreations {

        constructor() {
            this.canvas = document.getElementById('ic_canvas');
            this.renderer = new pc.Application(this.canvas, {
                mouse: new pc.Mouse(this.canvas),
                touch: !!('ontouchstart' in window) ? new pc.TouchDevice(this.canvas) : null,
                keyboard: new pc.Keyboard(window),
                gamepads: new pc.GamePads()
            });

            this.renderer.setCanvasFillMode(pc.FILLMODE_FILL_WINDOW);
            this.renderer.setCanvasResolution(pc.RESOLUTION_AUTO);
            this.renderer.on('update', this.update.bind(this));

            this.map = {
                size: {x: 500, y: 500, z: 0}
            }

            this.resize();

            this.scriptManager = new ScriptManager(pc);

            // register scripts
            if (this.scriptManager.loadScripts(['Camera', 'Controller', 'FlyCamera'])) {

                // create objects d
                this.objects = this.registerObjects();
            }



            // assign events 
            // register listeners
            this.registerListeners();

            // start socket
            this.sockets = [];
            this.socket = new WebSocket("ws://localhost:443");

            this.socket.onmessage = this.onMessage.bind(this);
            this.socket.onclose = this.onClose.bind(this);
            this.socket.onerror = this.onError.bind(this);

            // on open starts game
            this.socket.onopen = this.onOpen.bind(this);

        }

        onMessage() {
            console.log("Message " + e.data);
        }

        onClose() {
            alert("Socket closed");
        }

        onError(e) {
            alert("Failed to connect to socket > " + e)
        }

        onOpen() {
            this.run();
        }

        registerObjects(_) {
            // add game objects
            _ = {};

            _.floor = new pc.Entity('plane');
            _.floor.addComponent('model', { type: 'plane' })
            _.floor.addComponent('collision', {
                type: 'box',
                halfExtents: new pc.Vec3(this.map.size.x + 0.1, this.map.size.z + 0.1, this.map.size.y + 0.1)
            })

            console.log(_.floor)

            _.floor.setEulerAngles(90, 0, 0)
            _.floor.setLocalScale(this.map.size.x, this.map.size.z, this.map.size.y)
            _.floor.setPosition(0, 0, 0);

            _.camera = new pc.Entity('camera');
            _.light = new pc.Entity('light');

            // apply object properties


            _.camera.addComponent('script');
            _.camera.script.create('camera', { attributes: {} });


            _.light.addComponent('light');
            _.light.setEulerAngles(45, 0, 0);

            // add all objects to scene and return objects
            for (var i in _) {
                this.renderer.root.addChild(_[i]);
            }
            return _;
        }


        run() {
            this.resize();
            this.renderer.start();
            this.update();
        }

        registerListeners() {
            window.addEventListener('resize', this.resize.bind(this));
        }

        resize() {
            this.renderer.resizeCanvas()
        }

        update(dT) {

        }


    }


})