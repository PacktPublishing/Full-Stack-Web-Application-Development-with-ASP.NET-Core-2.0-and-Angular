export const pluckOut = (options: { key?: string, value:any, items: Array<any> }) => {    
    for (var i = 0; i < options.items.length; i++) {
        if (options.value == options.items[i][options.key || "id"]) {
            options.items.splice(i, 1);
        }
    }    
    return options.items.slice(0);
};