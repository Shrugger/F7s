using F7s.Mains;
using Stride.CommunityToolkit.Engine;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Rendering;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;
using System.Threading.Tasks;


using var game = new Game();
game.Run(start: Start, update: Update);

void Start (Scene scene) {

    Entity originEntity = new Entity("Origin");
    originEntity.Scene = scene;
    originEntity.Add(new MainSync());

    System.Diagnostics.Debug.WriteLine("Starting game " + game + " in scene " + scene + ".");

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

    game.Script.Scheduler.Add(Dissolve);
    async Task Dissolve () {

        while (true) {
            uiEntity.Remove();
            uiEntity.Dispose();
            await game.Script.NextFrame();
        }

    }
}

void Update (Scene scene, GameTime time) {

}