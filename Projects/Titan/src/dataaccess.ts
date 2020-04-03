var db = new PouchDB("titan");

const settingsKey: string = "settings";
const spriteSheetsKey: string = "spritesheets";

export interface ISettings {
    accent: string;
}

export interface ISpriteSheet {
    id: string;
    label: string;
    url: string;
    
    width?: number;
    height?: number;
    spriteWidth?: number;
    spriteHeight?: number;    
}

export function info() {
    db.info().then(info => {
        console.log(info);
    });
}

export function settings(): JQueryPromise<ISettings> {
    var d = $.Deferred<ISettings>();
    
    db.get(settingsKey).then(doc => {
        var settingsDoc = (<any>doc);
        d.resolve({ accent: settingsDoc.accent ? settingsDoc.accent : "" });   
    }).catch(error => {
        console.log(error);
        d.fail();
    })

    return d;
}

export function commitSetting(name: string, value: any) {
    db.get(settingsKey).then(doc => {
        doc[name] = value;
        db.put(doc);
    }).catch(error => {
        console.log(error);
    });
}

export function spritesheets(): JQueryPromise<ISpriteSheet[]> {
    var d = $.Deferred<ISpriteSheet[]>();
    
    db.get(spriteSheetsKey).then(doc => {
        var spriteSheets = [];
        var spriteSheetDoc = (<any>doc);
        spriteSheetDoc.spriteSheets.forEach(ss => {
            spriteSheets.push({
                id: ss.id,
                url: ss.url
            });
        });
        d.resolve(spriteSheets);   
    }).catch(error => {
        console.log(error);
        d.fail();
    });

    return d;
}

export function addSpritesheet(newSpriteSheet: ISpriteSheet) {
    db.get(spriteSheetsKey).then(doc => {
        var spriteSheetDoc = (<any>doc);
        spriteSheetDoc.spriteSheets.push(newSpriteSheet);
        db.put(spriteSheetDoc);
    }).catch(error => {
        console.log(error);
    });
}

function init() {
    db.get(settingsKey).then(doc => {
        console.log(doc._rev);
    }).catch(error => {
        if (error.status === 404) {
            // Document doesn't exist, so add it.
            let settings = {
                _id: settingsKey
            };

            db.put(settings);
        }

        console.log(error);
    });

    db.get(spriteSheetsKey).then(doc => {
        console.log(doc._rev);
    }).catch(error => {
        if (error.status === 404) {
            // Document doesn't exist, so add it.            
            let spritesheets = {
                _id: spriteSheetsKey
            };

            db.put(spritesheets);

            console.log(error);
        }
    });
}

export module TestData {
    export function spriteSheets(): ISpriteSheet[] {
        var spriteSheets = [];
        spriteSheets.push({
            id: "0",
            label: "zones",
            url: "C:\\Users\\noume\\Documents\\Code\\Abyss\\Art Assets\\HiDef\\Small Spritesheets\\zones.png" 
        });
        
        spriteSheets.push({
            id: "1",
            label: "doodads",
            url: "C:\\Users\\noume\\Documents\\Code\\Abyss\\Art Assets\\HiDef\\Small Spritesheets\\doodads_ogmo.png"
        });
        
        spriteSheets.push({
            id: "2",
            label: "debug",
            url: "C:\\Users\\noume\\Documents\\Code\\Abyss\\Art Assets\\HiDef\\Small Spritesheets\\debug.png"
        });
        
        spriteSheets.push({
            id: "3",
            label: "items",
            url: "C:\\Users\\noume\\Documents\\Code\\Abyss\\Art Assets\\HiDef\\Small Spritesheets\\items.png"
        });
        
        spriteSheets.push({
            id: "4",
            label: "monsters",
            url: "C:\\Users\\noume\\Documents\\Code\\Abyss\\Art Assets\\HiDef\\Small Spritesheets\\monsters.png"
        });
        
        spriteSheets.push({
            id: "5",
            label: "particles",
            url: "C:\\Users\\noume\\Documents\\Code\\Abyss\\Art Assets\\HiDef\\Small Spritesheets\\particles.png"
        });
        
        return spriteSheets;
    }
}

$(() => {
    // Initialise data access layer.
    init();
});