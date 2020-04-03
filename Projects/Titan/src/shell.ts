import * as Core from "./core";  

export interface IDimensions {
    width: number;
    height: number;
}

export function measure(site: JQuery, container: JQuery): IDimensions {
    // Clone the site first so we don't mess up any events and the like.
    var s = site.clone();

    // Attach an element to the dom but hide it so it can be measured properly.
    s.css({ visibility: "hidden", position: "absolute" });

    container.append(s);

    var height = s.outerHeight();
    var width = s.outerWidth();

    // Remove from the dom and revert any css properties.
    s.remove();
    s.css({ visibility: "", position: "" });

    return {
        height: height,
        width: width
    };
}

export interface ICoordinate {
    left?: number;
    top?: number;
    right?: number;
    bottom?: number; 
}

export interface IShellComponent {
    id: string;    
    site: JQuery;
    
    isVisible: boolean;
    shown: Core.Event<any>;
    hidden: Core.Event<any>;    
    
    init();
    layout();
    destroy();
}

export interface ICollapsible {
    isExpanded: boolean;
    expanded: Core.Event<any>;
    collapsed: Core.Event<any>;
}

export interface IToggleable {
    isOpen: boolean;
    opened: Core.Event<any>;
    closed: Core.Event<any>;    
}

export enum Direction {
    top,
    bottom,
    left,
    right
}

export interface IAffectLayout {
    id: string;
    direction: Direction;
}

export interface IGlyph {
    glyph: string;
}

export abstract class Glyph implements IShellComponent, IGlyph {       
    private _id: string;
    private _site: JQuery;
    private _glyph: string;
    
    private _isVisible: boolean;
    private _shown: Core.Event<any>;
    private _hidden: Core.Event<any>;
    
    constructor(id: string, site: JQuery, glyph: string) {
        this._id = id;
        this._site = site;
        this._glyph = glyph;
    }   
    
    get id(): string {
        return this._id;
    }
    
    get site(): JQuery {
        return this._site;
    }
    
    get glyph(): string {
        return this._glyph;
    }
    
    get isVisible(): boolean {
        return this._isVisible;
    }
    
    set isVisible(value: boolean) {
        if (this._isVisible === value) {
            return;
        }
        
        this._isVisible = value;
        this._isVisible ? this._shown.trigger() : this._hidden.trigger();
    }
    
    get shown(): Core.Event<any> {
        return this._shown;
    }
    
    get hidden(): Core.Event<any> {
        return this._hidden;
    }
    
    init() {
        this._shown = new Core.Event<any>();
        this._hidden = new Core.Event<any>();
        
        this._shown.on(() => { this.onShown() });
        this._hidden.on(() => { this.onHidden() });
    }
    
    layout() {
        var glyphSite = $(this.glyph).appendTo(this._site);
        this.isVisible = true;
    }
    
    destroy() {
        this._shown.off(() => { this.onShown() });
        this._hidden.off(() => { this.onHidden() });
    }
    
    protected onShown() {        
    } 
    
    protected onHidden() {        
    }
}

export abstract class ShellComponent implements IShellComponent, ICollapsible {
    private _id: string;
    
    private _container: JQuery;
    private _site: JQuery;
    
    private _isVisible: boolean;
    private _shown: Core.Event<any>;
    private _hidden: Core.Event<any>;
    
    private _isExpanded: boolean;
    private _expanded: Core.Event<any>;
    private _collapsed: Core.Event<any>;
    
    constructor(id: string, container: JQuery) {
        this._id = id;
        this._container = container;
    }
    
    get id(): string {
        return this._id;
    }
    
    get site(): JQuery {
        return this._site;
    }
    
    get isVisible(): boolean {
        return this._isVisible;
    }
    
    set isVisible(value: boolean) {
        if (this._isVisible === value) {
            return;
        }
        
        this._isVisible = value;
        this._isVisible ? this._shown.trigger() : this._hidden.trigger();
    }
    
    get shown(): Core.Event<any> {
        return this._shown;
    }
    
    get hidden(): Core.Event<any> {
        return this._hidden;
    }
    
    set isExpanded(value: boolean) {
        if (this._isExpanded === value) {
            return;
        }
        
        this._isExpanded = value;
        this._isExpanded ? this._expanded.trigger() : this._collapsed.trigger();
    }
    
    get expanded(): Core.Event<any> {
        return this._expanded;
    }
    
    get collapsed(): Core.Event<any> {
        return this._collapsed;
    }
    
    init() {    
        this._site = $(`<section id='${this.id}' class='sc'/>`).appendTo(this._container);
        
        this._shown = new Core.Event<any>();        
        this._hidden = new Core.Event<any>();
        
        this._expanded = new Core.Event<any>();
        this._collapsed = new Core.Event<any>();
        
        this._shown.on(() => { this.onShown(); });        
        this._hidden.on(() => { this.onHidden(); });
        
        this._expanded.on(() => { this.onExpanded(); });
        this._collapsed.on(() => { this.onCollapsed(); });
    }

    abstract layout();

    destroy() {
        this._shown.off(() => { this.onShown(); });
        this._hidden.off(() => { this.onHidden(); });
        this._expanded.off(() => { this.onExpanded(); });
        this._collapsed.off(() => { this.onCollapsed(); });
    }
    
    protected onShown() {        
    }
    
    protected onHidden() {        
    }
    
    protected onExpanded() {        
    }
    
    protected onCollapsed() {        
    }
}

export abstract class ToggleShellComponent extends ShellComponent implements IToggleable {
    private _isOpen: boolean;
    private _opened: Core.Event<any>;
    private _closed: Core.Event<any>;
    
    get isOpen(): boolean {
        return this._isOpen;
    }
    
    set isOpen(value: boolean) {
        if (this._isOpen === value) {
            return;
        }
        
        this._isOpen = value;
        this._isOpen ? this._opened.trigger() : this._closed.trigger();
    }
    
    get opened(): Core.Event<any> {
        return this._opened;
    }
    
    get closed(): Core.Event<any> {
        return this._closed;
    }
    
    init() {
        super.init();
        
        this._opened = new Core.Event<any>();
        this._closed = new Core.Event<any>();
        
        this._opened.on(() => { this.onOpened(); });
        this._closed.on(() => { this.onClosed(); });
    }
    
    destroy() {
        super.destroy();
        
        this._opened.off(() => { this.onOpened(); });
        this._closed.off(() => { this.onClosed(); });
    }
    
    protected onOpened() {        
    }
    
    protected onClosed() {        
    }
}

export interface IShell {
    site: JQuery;
    components: IShellComponent[];
    
    add(component: IShellComponent);
    get(id: string);
    
    init();
    layout();
    destroy();
}

export class Shell implements IShell {
    private _site: JQuery;
    
    private _components: IShellComponent[];
    private _componentsById: { [index: string]: IShellComponent };

    constructor(site: JQuery) {
        this._site = site;
    }

    get site(): JQuery {
        return this._site;
    }
    
    get components(): IShellComponent[] {
       return this._components;
    }
        
    add(component: IShellComponent) {
        this._components.push(component);
        this._componentsById[component.id] = component;
        
        component.init();
        component.layout();
    }
    
    get(id: string): IShellComponent {
        return this._componentsById[id];
    }

    init() {
        this._components = [];
        this._componentsById = {};
    }

    layout() {
    }

    destroy() {
        this._components.forEach(c => {
            c.destroy();
        });
    }
}