requirejs({
    baseUrl: '',
    paths: {
        Core: 'core',
        pc: 'assets/js/playcanvas-stable.min',
        _pc: 'assets/js/playcanvas-stable'
    }
})


requirejs(['Core/InfiniteCreations'], function (InfiniteCreations) {
    new InfiniteCreations();

})