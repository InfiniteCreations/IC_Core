define([], function () {

    return class EntityManager {

        constructor(pc) {
            this.pc = pc;
        }

        createEntity(name, entity, components, scripts) {
            if(Object.hasOwnProperty(this.entites, name)) 
                throw new Error("Cannot create entities with the same name");
            var entity = new this.pc.Entity(entity);
            entity.name = name;
            if (components instanceof Object) {
                for (var i in components) {
                    entity.addComponent(i, components[i] || {})
                }
            }
            if (scripts instanceof Object) {
                if(!entity.script) entity.addComponent('script');
                for (var i in scripts) {
                    entity.script.create(i, scripts[i] || {});
                }
            }
            return entity;
        }

    }

})