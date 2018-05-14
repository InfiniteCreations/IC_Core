define(['THREE'], function (THREE) {

    return class InfiniteCreations {

        constructor() {
            this.camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1e3);
            this.scene = new THREE.Scene();
            this.renderer = new THREE.WebGLRenderer();
            this.resize();

            document.body.appendChild(this.renderer.domElement);

            this.camera.position.z = 5;
            this.testrender();
            window.addEventListener('resize', this.resize.bind(this));
            this.update();
        }


        testrender() {
            var geo = new THREE.SphereGeometry(1, 1, 1);
            geo = new THREE.EdgesGeometry(geo);
            var mat = new THREE.LineBasicMaterial({ color: 0x00ff00, linewidth: 2 });
            var cube = new THREE.LineSegments(geo, mat);
            this.scene.add(cube);

        }

        resize() {
            this.renderer.setSize(window.innerWidth, window.innerHeight);
        }

        update() {
            window.requestAnimationFrame(this.update.bind(this));
            this.scene.children[0].rotation.x += 0.01;
            this.scene.children[0].rotation.y += 0.01;
            this.renderer.render(this.scene, this.camera);
        }

    }

})