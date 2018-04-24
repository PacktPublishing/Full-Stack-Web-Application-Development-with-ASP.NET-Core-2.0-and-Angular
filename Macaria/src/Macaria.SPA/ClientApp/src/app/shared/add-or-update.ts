export const addOrUpdate = (options: { items: Array<any>, item: any, key?: string, mode?:string }) => {
    let exists = false;
    options.items = options.items || [];

    if (options.mode) {
        if (options.items.indexOf(options.item) < 0)
            options.items.push(options.item);            
        return options.items.slice(0);
    }

    for (let i = 0; i < options.items.length; i++) {
        if (options.items[i][options.key || "id"] === options.item[options.key || "id"]) {
            options.items[i] = options.item;
            exists = true;
        }
    }
    if (!exists) {
        options.items.push(options.item);        
    }
    return options.items.slice(0);
}