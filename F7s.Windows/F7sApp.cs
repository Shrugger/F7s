using Stride.CommunityToolkit.Engine;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Rendering;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;

using var game = new Game();
game.Run(start: Start, update: Update);

void Start (Scene scene) {

    SpriteFont? font = null;
    font = game.Content.Load<SpriteFont>("StrideDefaultFont");
    var canvas = new Canvas {
        Width = 300,
        Height = 100,
        BackgroundColor = new Color(248, 177, 149, 100),
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Bottom,
    };

    canvas.Children.Add(new TextBlock {
        Text = "Hello, Stride!",
        TextColor = Color.White,
        Font = font,
        TextSize = 24,
        Margin = new Thickness(3, 3, 3, 0),
    });

    var uiEntity = new Entity
    {
        new UIComponent
        {
            Page = new UIPage { RootElement = canvas },
            RenderGroup = RenderGroup.Group31
        }
    };

    uiEntity.Scene = scene;
}

void Update (Scene scene, GameTime time) {

}