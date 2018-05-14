requirejs({
    baseUrl: '',
    paths: {
        scripts: 'assets/js/scripts',
        THREE: 'assets/js/three.min'
    }
})


requirejs(['scripts/InfiniteCreations'], function (InfiniteCreations) {

    new InfiniteCreations();
})