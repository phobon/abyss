/// <reference path="core.d.ts" />

interface IDimensions {
    width: number;
    height: number;
}

interface ICoordinate {
    left?: number;
    top?: number;
    right?: number;
    bottom?: number; 
}

interface IShellComponent {
    id: string;    
    site: JQuery;
    
    isVisible: boolean;
    shown: IEvent<any>;
    hidden: IEvent<any>;    
    
    init();
    layout();
    destroy();
}

interface ICollapsible {
    isExpanded: boolean;
    expanded: IEvent<any>;
    collapsed: IEvent<any>;
}

interface IToggleable {
    isOpen: boolean;
    opened: IEvent<any>;
    closed: IEvent<any>;    
}

interface IGlyph {
    glyph: string;
}

interface IShell {
    site: JQuery;
    components: IShellComponent[];
    
    add(component: IShellComponent);
    get(id: string);
    
    init();
    layout();
    destroy();
}