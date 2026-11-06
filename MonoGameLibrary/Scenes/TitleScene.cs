using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace EternalPlague.Scenes;

public class TitleScene : Scene
{
private const string Eternal_TEXT = "Eternal";
private const string Plague_TEXT = "Plague";
private const string PRESS_ENTER_TEXT = "Presione 'Enter' para comenzar";
private SpriteFont _font;
private Vector2 _dungeonTextPos;

private Vector2 _dungeonTextOrigin;

private Vector2 _slimeTextPos;

// The origin to set for the slime text.
private Vector2 _slimeTextOrigin;

// The position to draw the press enter text at.
private Vector2 _pressEnterPos;

// The origin to set for the press enter text when drawing it.
private Vector2 _pressEnterOrigin;



public override void Initialize()
{
    // LoadContent is called during base.Initialize().
    base.Initialize();

    // While on the title screen, we can enable exit on escape so the player
    // can close the game by pressing the escape key.
    Core.ExitOnEscape = true;

    // Set the position and origin for the Dungeon text.
    Vector2 size = _font.MeasureString(Eternal_TEXT);
    _dungeonTextPos = new Vector2(640, 100);
    _dungeonTextOrigin = size * 0.5f;

    // Set the position and origin for the Slime text.
    size = _font.MeasureString(Plague_TEXT);
    _slimeTextPos = new Vector2(640, 150);
    _slimeTextOrigin = size * 0.5f;

    // Set the position and origin for the press enter text.
    size = _font.MeasureString(PRESS_ENTER_TEXT);
    _pressEnterPos = new Vector2(640, 520);
    _pressEnterOrigin = size * 0.5f;
    
}
    public override void LoadContent()
    {
         _font = Content.Load<SpriteFont>("Font/FontPrincipal");
    }
    public override void Update(GameTime gameTime)
{
    // If the user presses enter, switch to the game scene.
    if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
    {
        Core.ChangeScene(new GameScene());
    }
}
public override void Draw(GameTime gameTime)
{
    Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

    // Begin the sprite batch to prepare for rendering.
    Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

    // The color to use for the drop shadow text.
    Color dropShadowColor = Color.Black * 0.5f;

    // Draw the Dungeon text slightly offset from it is original position and
    // with a transparent color to give it a drop shadow.
    Core.SpriteBatch.DrawString(_font, Eternal_TEXT, _dungeonTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, _dungeonTextOrigin, 2.0f, SpriteEffects.None, 1.0f);

    // Draw the Dungeon text on top of that at its original position.
    Core.SpriteBatch.DrawString(_font, Eternal_TEXT, _dungeonTextPos, Color.White, 0.0f, _dungeonTextOrigin, 2.0f, SpriteEffects.None, 1.0f);

    // Draw the Slime text slightly offset from it is original position and
    // with a transparent color to give it a drop shadow.
    Core.SpriteBatch.DrawString(_font, Plague_TEXT, _slimeTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, _slimeTextOrigin, 2.0f, SpriteEffects.None, 1.0f);

    // Draw the Slime text on top of that at its original position.
    Core.SpriteBatch.DrawString(_font, Plague_TEXT, _slimeTextPos, Color.White, 0.0f, _slimeTextOrigin, 2.0f, SpriteEffects.None, 1.0f);

    // Draw the press enter text.
    Core.SpriteBatch.DrawString(_font, PRESS_ENTER_TEXT, _pressEnterPos, Color.White, 0.0f, _pressEnterOrigin, 2.0f, SpriteEffects.None, 0.0f);

    // Always end the sprite batch when finished.
    Core.SpriteBatch.End();
}



}
