using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace Eternal_Plague;

public class Game1 : Core
{
    private Texture2D fumigador;
    private Vector2 posicion,origen;
    private float rotar;

    private Tilemap _piso;

    private Tilemap _paredes;

    public Game1() : base("Eternal Plague", 1280, 832, false)
    {

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        fumigador=Content.Load<Texture2D>("images/Fumigador");
        posicion=new Vector2(100,100);
       
        // _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");
        // _tilemap.Scale = Vector2.One;

        _piso = Tilemap.FromFile(Content, "images/tilemap-piso.xml");
        _paredes = Tilemap.FromFile(Content, "images/tilemap-paredes.xml");


        // TODO: use this.Content to load your game content here
    }





    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
              var kstate = Keyboard.GetState();
    // Movimiento con WASD o flechas
    if (kstate.IsKeyDown(Keys.W))
    {
        posicion.Y = posicion.Y-2;
        
    }
    if (kstate.IsKeyDown(Keys.S) )
    {
        posicion.Y = posicion.Y+2 ;
    }
    if (kstate.IsKeyDown(Keys.A) )
    {
        posicion.X = posicion.X-2 ;
    }
    if (kstate.IsKeyDown(Keys.D) )
    {
        posicion.X = posicion.X+2 ; 
    }


    if ( kstate.IsKeyDown(Keys.Up))
    {
        rotar=0.0f;
        
    }
    if ( kstate.IsKeyDown(Keys.Down))
    {
        rotar=3.14f  ;
    }
    if (kstate.IsKeyDown(Keys.Left))
    {
        rotar = 3.14f * 1.5f;
    }
    if (kstate.IsKeyDown(Keys.Right))
    {
        rotar = 3.14f*0.5f; 
    }

        // TODO: Add your update logic here
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
         origen=new Vector2(fumigador.Width/2.0f,fumigador.Height/2.0f);

        SpriteBatch.Begin(
            samplerState: SamplerState.PointClamp
        );


        _piso.Draw(SpriteBatch);
        _paredes.Draw(SpriteBatch);

        SpriteBatch.Draw(fumigador,posicion,null,Color.White,rotar,origen,1.0f,SpriteEffects.None,0.0f);
        SpriteBatch.End();

        base.Draw(gameTime);

    }
}
