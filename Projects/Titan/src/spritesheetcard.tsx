import React = require("react");
import ReactDOM = require("react-dom");

class SpriteSheetCard extends React.Component<any, any> {
    render(): any {
        return (
            <div>
                <h2>test</h2>
            </div>
        );
    }
}

ReactDOM.render(
    <SpriteSheetCard/>
, $("body")[0]);
