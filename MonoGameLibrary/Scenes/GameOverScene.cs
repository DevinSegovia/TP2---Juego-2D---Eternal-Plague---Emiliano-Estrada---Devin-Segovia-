using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace EternalPlague.Scenes;

public class GameOverScene : Scene
{

    private const string Game_TEXT = "Game Over";
    private const string PRESS_ENTER_TEXT = "Presione 'Enter' para continuar";
    private const string EXIT_TEXT = "Presione 'Q' para salir";
    private SpriteFont _font;

    private Vector2 _gameTextPos;

    private Vector2 _gameTextOrigin;


    private Vector2 _pressEnterPos;

    // The origin to set for the press enter text when drawing it.
    private Vector2 _pressEnterOrigin;

    private Vector2 _exitPos;
    private Vector2 _exitOrigin;



    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = true;

        // Set the position and origin for the Dungeon text.
        Vector2 size = _font.MeasureString(Game_TEXT);
        _gameTextPos = new Vector2(640, 150);
        _gameTextOrigin = size * 0.5f;

        // Set the position and origin for the Slime text.
        // size = _font.MeasureString(Over_TEXT);
        // _overTextPos = new Vector2(640, 150);
        // _overTextOrigin = size * 0.5f;

        // Set the position and origin for the press enter text.
        size = _font.MeasureString(PRESS_ENTER_TEXT);
        _pressEnterPos = new Vector2(640, 520);
        _pressEnterOrigin = size * 0.5f;

        size = _font.MeasureString(EXIT_TEXT);
        _exitPos = new Vector2(640, 600);
        _exitOrigin = size * 0.5f;


    }


    public override void LoadContent()
    {
        _font = Content.Load<SpriteFont>("Font/FontPrincipal");
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            Core.ChangeScene(new GameScene());
        }

        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Q))
        {
            Core.ChangeScene(new TitleScene());
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(88, 24, 69, 255));

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // The color to use for the drop shadow text.
        Color dropShadowColor = Color.OrangeRed * 3f;

        // Draw the Dungeon text slightly offset from it is original position and
        // with a transparent color to give it a drop shadow.
        Core.SpriteBatch.DrawString(_font, Game_TEXT, _gameTextPos + new Vector2(4, 4), dropShadowColor, 0.0f, _gameTextOrigin, 3.5f, SpriteEffects.None, 1.0f);

        // Draw the Dungeon text on top of that at its original position.
        Core.SpriteBatch.DrawString(_font, Game_TEXT, _gameTextPos, Color.White, 0.0f, _gameTextOrigin, 3.5f, SpriteEffects.None, 1.0f);

        // Draw the Slime text slightly offset from it is original position and
        // with a transparent color to give it a drop shadow.
        // Core.SpriteBatch.DrawString(_font, Over_TEXT, _overTextPos + new Vector2(4, 4), dropShadowColor, 0.0f, _overTextOrigin, 2.5f, SpriteEffects.None, 1.0f);

        // // Draw the Slime text on top of that at its original position.
        // Core.SpriteBatch.DrawString(_font, Over_TEXT, _overTextPos, Color.White, 0.0f, _overTextOrigin, 2.5f, SpriteEffects.None, 1.0f);

        // Draw the press enter text.
        Core.SpriteBatch.DrawString(_font, PRESS_ENTER_TEXT, _pressEnterPos, Color.White, 0.0f, _pressEnterOrigin, 2f, SpriteEffects.None, 0.0f);

        Core.SpriteBatch.DrawString(_font, EXIT_TEXT, _exitPos, Color.White, 0.0f, _exitOrigin, 2f, SpriteEffects.None, 0.0f);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
}