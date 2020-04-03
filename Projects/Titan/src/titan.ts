import { IShellComponent, ShellComponent, IShell, Shell, IGlyph, Glyph } from "./shell";
import * as Core from "./core";
import * as DataAccess from "./dataaccess";
var TestData = DataAccess.TestData;

class ComponentKeys {
    static header: string = "header";
    static collection: string = "collection";
    static navigator: string = "navigator";
    static detail: string = "detail";
}

class Glyphs {
    static particle: string = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"><path d="M16 0A16 16 0 1 0 32 16 16 16 0 0 0 16 0ZM11.67 17.33a1 1 0 0 1-1 1H8a1 1 0 0 1-1-1V14.67a1 1 0 0 1 1-1h2.66a1 1 0 0 1 1 1v2.66ZM18.33 24a1 1 0 0 1-1 1H14.67a1 1 0 0 1-1-1V21.33a1 1 0 0 1 1-1h2.66a1 1 0 0 1 1 1V24Zm0-13.32a1 1 0 0 1-1 1H14.67a1 1 0 0 1-1-1V8a1 1 0 0 1 1-1h2.66a1 1 0 0 1 1 1v2.66ZM25 17.33a1 1 0 0 1-1 1H21.33a1 1 0 0 1-1-1V14.67a1 1 0 0 1 1-1H24a1 1 0 0 1 1 1v2.66Z"/><rect style="fill:#fff;" x="13.67" y="7.01" width="4.66" height="4.66" rx="1" ry="1"/><rect style="fill:#fff;" x="20.33" y="13.67" width="4.66" height="4.66" rx="1" ry="1"/><rect style="fill:#fff;" x="13.67" y="20.33" width="4.66" height="4.66" rx="1" ry="1"/><rect style="fill:#fff;" x="7.01" y="13.67" width="4.66" height="4.66" rx="1" ry="1"/></svg>';

    static animation: string = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"><circle cx="16" cy="16" r="16"/><circle cx="21.53" cy="15.47" r="3.45" fill="#fff"/><path d="M21.53 15.47c-3.12 0-5.46 3.41-5.46 7.94" style="fill:none;stroke-width:1.5;stroke:#fff"/><path d="M7 9.68c5.17 0 9.06 5.9 9.06 13.73" style="fill:none;stroke-width:1.5;stroke:#fff"/></svg>';

    static sprite: string = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"><path d="M16,0A16,16,0,1,0,32,16,16,16,0,0,0,16,0Zm8.47,17.41L16,25.88,7.53,17.41A5.43,5.43,0,1,1,16,10.73,5.43,5.43,0,1,1,24.47,17.41Z"/><path style="fill:#fff;" d="M20.64,8.12A5.43,5.43,0,0,0,16,10.73a5.43,5.43,0,1,0-8.47,6.67L16,25.88l8.47-8.47A5.43,5.43,0,0,0,20.64,8.12Z" /></svg>';

    static doodad: string = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"><path d="M16,0A16,16,0,1,0,32,16,16,16,0,0,0,16,0Zm0,24a8,8,0,1,1,8-8A8,8,0,0,1,16,24Z"/><circle style="fill:#fff;" cx="16" cy="16" r="8"/></svg>';

    static hamburger: string = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 29.21 22.04"><line style="stroke-linecap:round;stroke-miterlimit:10;stroke-width:4px;" x1="27.21" y1="20.04" x2="2" y2="20.04"/><line style="stroke-linecap:round;stroke-miterlimit:10;stroke-width:4px;" x1="27.21" y1="2" x2="2" y2="2"/><line style="stroke-linecap:round;stroke-miterlimit:10;stroke-width:3px;" x1="27.21" y1="11.02" x2="2" y2="11.02"/></svg>';

    static bookmark: string = '';

    static favourite: string = '';

    static plus: string = '';
}

class TitanGlyph extends Glyph {
    private static markup: string = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 48 48"><polygon points="0 48 0 0 40.63 0 40.63 22.11 33.26 14.74 33.26 7.37 7.37 7.37 7.37 40.63 0 48"/></svg>';

    constructor(site: JQuery) {
        super("brand", site, TitanGlyph.markup);
    }

    layout() {
        super.layout();

        this.site.on("click", () => {
            this.isVisible = !this.isVisible;
        });
    }

    destroy() {
        this.site.off("click");
    }

    protected onShown() {
        this.site.find("svg").velocity("transition.fadeIn");
    }

    protected onHidden() {
        this.site.find("svg").velocity("transition.fadeOut");
    }
}

class Header extends ShellComponent {
    private glyph: Glyph;

    constructor(site: JQuery) {
        super(ComponentKeys.header, site)
    }

    layout() {
        var headerList = $("<ul></ul>").appendTo(this.site);

        var glyphSite = $("<li class='titan-glyph'></li>").appendTo(headerList);
        this.glyph = new TitanGlyph(glyphSite);
        this.glyph.init();
        this.glyph.layout();

        editor.get(ComponentKeys.navigator).site.css("padding-top", this.site.outerHeight());
        editor.get(ComponentKeys.collection).site.css("padding-top", this.site.outerHeight());
    }
}

class Collection extends ShellComponent {
    private _content: JQuery;
    private _addSpritesheetButton: JQuery;

    constructor(site: JQuery) {
        super(ComponentKeys.collection, site);
    }

    layout() {
        // TODO: Get the spritesheets.
        var spriteSheets: DataAccess.ISpriteSheet[] = TestData.spriteSheets();
        
        // Layout the spritesheets as necessary.
        this._content = $("<ul/>").appendTo(this.site);
        spriteSheets.forEach(spriteSheet => {
            let container = $("<li/>").appendTo(this._content);
            let detail = $("<ul class='detail'/>").appendTo(container);
            
            // Build the header.            
            let header = $(`<li class='header'/>`).appendTo(detail);
            let headerContainer = $(`<ul><li class='name'><h2>${spriteSheet.label}</h2></li></ul>`).appendTo(header);

            let headerMenu = $(`<li class='settings'>${Glyphs.hamburger}</li>`).appendTo(headerContainer);
            headerMenu.on("click", () => {
                alert(`menu clicked for: ${spriteSheet.label}`);
            });
            
            // Add the image.
            var url = "file:///" + spriteSheet.url.split("\\").join("/");
            var previewSite = $("<li class='preview'/>").appendTo(detail);
            var preview = $("<div/>").appendTo(previewSite);
            preview.css({ "background-image": `url('${url}')`});
            
            // TODO: Add the number of sprites/doodads/whatever.
            let spriteDetailSite = $("<li class='statistics'/>").appendTo(detail);
            let spriteDetails = $("<ul/>").appendTo(spriteDetailSite);
            let spriteCount = $(`<li>${Glyphs.sprite}<span>0</span></li><li>${Glyphs.doodad}<span>0</span></li><li>${Glyphs.particle}<span>0</span></li>`).appendTo(spriteDetails);
        
            // Add the bookmark/favourite site.
            let miscSite = $("<li class='misc'/>").appendTo(detail);
            let misc = $("<ul/>").appendTo(miscSite);
            let bookmark = $(`<li>${Glyphs.bookmark}</li>`).appendTo(misc);
            bookmark.on("click", () => {
                alert("bookmark clicked");
            });

            let favourite = $(`<li>${Glyphs.favourite}</li>`).appendTo(misc);
            favourite.on("click", () => {
                alert("favourite clicked");
            });
        });
        
        // Add a 'new spritesheet' button.
        this._addSpritesheetButton = $(`<li class='add-spritesheet'>${Glyphs.plus}</li>`).appendTo(this._content);
        this._addSpritesheetButton.on("click", () => {
            alert("add new spritesheet clicked");
        });
    }
}

class Navigator extends ShellComponent {
    constructor(site: JQuery) {
        super(ComponentKeys.navigator, site)
    }

    init() {
        super.init();
    }

    layout() {
        var items = $("<ul/>").appendTo(this.site);

        // this.refresh().done(spriteSheets => {
        //     spriteSheets.forEach(i => {
        //         var newSite = $(`<li>${i}</li>`).appendTo(items);
        //         newSite.click(() => {
        //             console.log(`item: ${i} activated.`);
        //         })
        //     });
        // });
    }

    refresh(): JQueryPromise<DataAccess.ISpriteSheet[]> {
        var d = $.Deferred<DataAccess.ISpriteSheet[]>();

        DataAccess.spritesheets().done(spriteSheets => {
            d.resolve(spriteSheets);
        }).fail(err => {
            d.fail(err);
        })

        return d;
    }
}

var editor: TitanEditor;

enum EditorMode {
    Collection,
    Detail
}

class TitanEditor extends Shell {
    private _currentSpriteSheet: DataAccess.ISpriteSheet;

    private _mode: EditorMode;
    private _modeChanged: Core.IEvent<EditorMode>;

    get currentSpriteSheet(): DataAccess.ISpriteSheet {
        return this._currentSpriteSheet;
    }

    set currentSpriteSheet(value: DataAccess.ISpriteSheet) {
        if (value === this._currentSpriteSheet) {
            return;
        }

        this._currentSpriteSheet = value;
        this._currentSpriteSheet ? this.mode = EditorMode.Detail : this.mode = EditorMode.Collection;
    }

    get mode(): EditorMode {
        return this._mode;
    }

    set mode(value: EditorMode) {
        if (value === this._mode) {
            return;
        }

        this._mode = value;
        this._modeChanged.trigger(this._mode);
    }

    init() {
        super.init();

        this._modeChanged = new Core.Event<EditorMode>();
        this._modeChanged.on(mode => { this.onModeChanged(mode); });
    }

    destroy() {
        super.destroy();

        this._modeChanged.off(mode => { this.onModeChanged(mode); });
    }

    private onModeChanged(mode: EditorMode) {
        switch (mode) {
            case EditorMode.Collection:
                this.collectionMode();
                break;
            case EditorMode.Detail:
                this.detailMode();
                break;
        }
    }

    private collectionMode() {
        // Remove the current sprite sheet so we can set the mode easily later.
        if (this._currentSpriteSheet) {
            this._currentSpriteSheet = null;
        }
        
        // Collapse the navigator and detail, show the collection view.
        var navigator = this.get(ComponentKeys.navigator);
        var detail = this.get(ComponentKeys.detail);
        var collection = this.get(ComponentKeys.collection);
    }

    private detailMode() {        
        // Collapse the collection view, show the navigator and detail views.
        var navigator = this.get(ComponentKeys.navigator);
        var detail = this.get(ComponentKeys.detail);
        var collection = this.get(ComponentKeys.collection);
    }
}

$(() => {
    var body = $("body");

    editor = new TitanEditor(body);
    editor.init();
    editor.layout();

    editor.add(new Collection(body));
    editor.add(new Navigator(body));
    editor.add(new Header(body));    
    
    // Debugging.
    // DataAccess.commitSetting("accent", "blue");
    // DataAccess.settings().done(settings => {
    // });
});