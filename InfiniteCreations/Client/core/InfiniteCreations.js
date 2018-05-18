define(['_pc', 'Core/ScriptManager', 'Core/EntityManager'], function (pc, ScriptManager, EntityManager) {

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
                size: {x: 1000, y: 1000, z: 0}
            }


            this.resize();

            this.scriptManager = new ScriptManager(pc);
            this.entityManager = new EntityManager(pc);

            // register scripts
            if (this.scriptManager.loadScripts(['Camera'])) {
                this.objects = this.registerObjects();
            }



            // assign events 
            // register listeners
            this.registerListeners();

            // start socket
            this.sockets = [];
            this.socket = new WebSocket("ws://host.legitsoulja.info:443");

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
            _ = {}

            // create game objects

            _.player = this.entityManager.createEntity('player', null,
                {
                    model: {
                        type: 'asset',
                        castShadows: true,
                        isStatic: false,
                        receiveShadows: false
                    },
                    animation: {
                        loop: true,
                        activate: true,
                        enabled: false
                    }
                });

            _.floor = this.entityManager.createEntity('ground', 'plane', {
                model: { type: 'plane' },
                collision: {
                    type: 'box',
                    halfExtents: new pc.Vec3(this.map.size.x / 2, this.map.size.z / 2, this.map.size.y / 2)
                }
            });

            _.camera = this.entityManager.createEntity('debugcamera', 'camera', {},
                {
                    camera: { attributes: {} }
                });

            _.light = this.entityManager.createEntity('light', 'light', { light: null });

            // apply object properties
            _.light.setPosition(0, 50, 0);
            _.floor.setPosition(0, 0, 0);
            _.floor.setEulerAngles(0, 0, 0)
            _.floor.setLocalScale(this.map.size.x, this.map.size.z, this.map.size.y)
            _.light.setEulerAngles(45, 0, 0);


            // load player object/textures

            this.renderer.assets.loadFromUrl('public/models/player/player.json', 'model', function (error, asset) {
                if (error) {
                    alert("Failed to load " + t);
                    return;
                }


                _.player.model.asset = asset;
                _.player.setLocalScale(3, 3, 3);
                
                // Load animations

                var animations = [
                    'idle:public/animations/idle/idle.json',
                    'jog:public/animations/jog/jog.json',
                    'running:public/animations/running/running.json'
                ]

                animations.map(t => {
                    let s = t.split(':');
                    let asset = new pc.Asset(s[0], 'animation', { url: s[1] });
                    this.renderer.assets.load(asset);
                    _.player.animation.animations[s[0]] = undefined;
                    asset.on('load', function () {
                        _.player.animation.animations[s[0]] = asset.resource;
                        _.player.animation.enabled = false;
                        _.player.animation.enabled = true;
                    })
                });

                _.player.animation.play('idle');

            }.bind(this))

            window.player = _.player;

            // setup a skybox environment

            var textures = [
                    'public/cubemaps/yokohama/posx.jpg',
                    'public/cubemaps/yokohama/negx.jpg',
                    'public/cubemaps/yokohama/posy.jpg',
                    'public/cubemaps/yokohama/negy.jpg',
                    'public/cubemaps/yokohama/posz.jpg',
                    'public/cubemaps/yokohama/negz.jpg',
            ]


            textures = textures.map(t => {
                var asset = new pc.Asset(t, 'texture',
                  { url: t },
                  { addressu: 'repeat', addressv: 'repeat' }
                );

                this.renderer.assets.add(asset);
                this.renderer.assets.load(asset);
                return asset.id;
            });




            this.renderer.setSkybox(new pc.Asset('skybox', 'cubemap',
                null,
                {
                    anisotropy: 1,
                    magFilter: 1,
                    minFilter: 5,
                    rgbm: false,
                    textures: textures
                }
            ))

            // add all objects to scene and return objects
            for (var i in _) {
                this.renderer.root.addChild(_[i]);
            }
            return _;
        }


        run() {
            this.resize();
            this.renderer.start();
            this.objects.player.animation.enabled = true;
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