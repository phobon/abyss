export interface IEvent<T> {
    on(handler: { (data?: T): void });
    off(handler: { (data?: T): void });
    trigger(data?: T);
}

export class Event<T> implements IEvent<T> {
    private _handlers: { (data?: T): void; }[];

    public on(handler: { (data?: T): void }) {
        if (!this._handlers) {
            this._handlers = [];
        }
        
        this._handlers.push(handler);
    }

    public off(handler: { (data?: T): void }) {
        if (this._handlers) {            
            this._handlers.splice(this._handlers.indexOf(handler), 1);
        }
    }

    public trigger(data?: T) {
        if (this._handlers) {
            this._handlers.forEach(h => h(data)); 
        }
    }
}