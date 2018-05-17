requirejs({
    baseUrl: '',
    paths: {
        Core: 'core',
        _pc: 'assets/js/playcanvas-stable.min',
        pc: 'assets/js/playcanvas-stable'
    }
})


requirejs(['Core/InfiniteCreations'], function (InfiniteCreations) {
    new InfiniteCreations();

})