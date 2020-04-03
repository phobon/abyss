interface IEvent<T> {
    on(handler: { (data?: T): void });
    off(handler: { (data?: T): void });
    trigger(data?: T);
}