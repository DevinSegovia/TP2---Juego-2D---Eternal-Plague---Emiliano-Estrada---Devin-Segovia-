using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary;
using System.Collections.Generic;
using MonoGameLibrary.Scenes;
using System.Security.Cryptography.X509Certificates;

namespace EternalPlague.Scenes;

public class GameScene : Scene
{
private Texture2D fumigador,serpiente;
    private Vector2 _fum_posicion, _fum_origen,_fum_posicion_mal,disparo,_fumigadorvelocidad,posicio;
    private float rotar;
    private bool isactive{get;set;}=false;
    private static TimeSpan s_duraciondisparo= TimeSpan.FromMilliseconds(2000);
    private TimeSpan duraciondisparo=TimeSpan.Zero;


    private List<Rectangle> _colisiones = new List<Rectangle>();

    private Tilemap _piso;

    private Tilemap _paredes;

    private int _fumAncho = 24;
    private int _fumAlto = 24;
    private bool matarenemigo(Vector2 posicion, Vector2 posicion2)
    {
        Rectangle disparo = new Rectangle(
            (int)posicion.X - _fumAncho *2,
            (int)posicion.Y - _fumAlto *2, _fumAncho*2, _fumAlto*2
            );
            Rectangle enemigo = new Rectangle(
            (int)posicion2.X - _fumAncho / 2,
            (int)posicion2.Y - _fumAlto / 2, _fumAncho, _fumAlto
            );
            
            if (disparo.Intersects(enemigo))
            {
            return true;
            }
                
        return false;
    }
     private bool ChocaConPared(Vector2 posicion) //fija al jugador si choca con la pared
    {
        Rectangle jugador = new Rectangle(
            (int)posicion.X - _fumAncho / 2,
            (int)posicion.Y - _fumAlto / 2, _fumAncho, _fumAlto
            );

        foreach (Rectangle pared in _colisiones)
        {
            if (jugador.Intersects(pared))
                return true;
        }

        return false;
    }

    private SpriteFont font;
    public override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
        _fum_posicion_mal=new Vector2(192,128);
        AssignRandomfum();
    }
     public override void LoadContent()
    {
        //Texto principal
        font = Content.Load<SpriteFont>("Font/FontPrincipal");

        fumigador = Content.Load<Texture2D>("images/Fumigador");
        serpiente=Content.Load<Texture2D>("images/serpiente");
        _fum_posicion = new Vector2(50, 80);

        // _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");
        // _tilemap.Scale = Vector2.One;

        _piso = Tilemap.FromFile(Content, "images/tilemap-piso.xml");
        _paredes = Tilemap.FromFile(Content, "images/tilemap-paredes.xml");

        // COLISIONES PAREDES

        TextureRegion vacio = _paredes.GetTile(0, 0);   // la celda (0,0) es un 120 o sea null, palabras simples piso.

        for (int f = 0; f < _paredes.Rows; f++)
        {
            for (int c = 0; c < _paredes.Columns; c++)
            {
                if (_paredes.GetTile(c, f) != vacio)
                {
                    _colisiones.Add(new Rectangle(
                        c * (int)_paredes.TileWidth,
                        f * (int)_paredes.TileHeight,
                        (int)_paredes.TileWidth,
                        (int)_paredes.TileHeight));
                }
            }
        }


        // TODO: use this.Content to load your game content here
    }
    public override void Update(GameTime gameTime)
    {
        CheckKeyboardInput();
        Vector2 posAnterior = _fum_posicion;
        if (matarenemigo(disparo,_fum_posicion_mal))
        {
            _fum_posicion_mal=new Vector2();
        }
        if (ChocaConPared(_fum_posicion))
            _fum_posicion.Y = posAnterior.Y;

        if (ChocaConPared(_fum_posicion))
            _fum_posicion.X = posAnterior.X;

        if(isactive==true){
        duraciondisparo+=TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.Milliseconds);
        }
        if (duraciondisparo > s_duraciondisparo)
        {
            duraciondisparo=TimeSpan.FromMilliseconds(0);
            isactive=false;
        }

       
        // TODO: Add your update logic here
        base.Update(gameTime);
    }
    private void AssignRandomfum(){
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

        // Convert angle to a direction vector.
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        _fumigadorvelocidad = direction * 1.0f;
        }
    private void CheckKeyboardInput()
    {
    // Get a reference to the keyboard inof
    KeyboardInfo keyboard = Core.Input.Keyboard;

    // If the escape key is pressed, return to the title screen.
    if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
    {
        Core.ChangeScene(new TitleScene());
    }

    if (keyboard.IsKeyDown(Keys.W)  )
    {
        _fum_posicion.Y = _fum_posicion.Y- 2;
    }
        if (keyboard.IsKeyDown(Keys.Up))
        {
            rotar = 0.0f;
            posicio.X=_fum_posicion.X;
            posicio.Y=_fum_posicion.Y;
            disparo=new Vector2(posicio.X-16,posicio.Y-16);
            isactive=true;
        }

    // if the S or Down keys are down, move the slime down on the screen.
    if (keyboard.IsKeyDown(Keys.S)  )
    {
        _fum_posicion.Y = _fum_posicion.Y+ 2;
    }
        if (keyboard.IsKeyDown(Keys.Down))
        {
            rotar = 3.14f;
            posicio.X=_fum_posicion.X;
            posicio.Y=_fum_posicion.Y;
            disparo=new Vector2(posicio.X+12,posicio.Y+12);
            isactive=true;
        }

    // If the A or Left keys are down, move the slime left on the screen.
    if (keyboard.IsKeyDown(Keys.A)  )
    {
        _fum_posicion.X = _fum_posicion.X- 2;
    }
        if (keyboard.IsKeyDown(Keys.Left))
        {
             rotar = 3.14f * 1.5f;
            posicio.X=_fum_posicion.X;
            posicio.Y=_fum_posicion.Y;
            disparo=new Vector2(posicio.X-16,posicio.Y+8);
            isactive=true;
        }

    // If the D or Right keys are down, move the slime right on the screen.
    if (keyboard.IsKeyDown(Keys.D)  )
    {
        _fum_posicion.X = _fum_posicion.X+ 2;
    }
        if (keyboard.IsKeyDown(Keys.Right))
        {
             rotar = 3.14f * 0.5f;
            posicio.X=_fum_posicion.X;
            posicio.Y=_fum_posicion.Y;
            disparo=new Vector2(posicio.X+12,posicio.Y-16);
            isactive=true;
        }

}
public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);
        _fum_origen = new Vector2(fumigador.Width / 2.0f, fumigador.Height / 2.0f);
        Core.SpriteBatch.Begin(
            samplerState: SamplerState.PointClamp
        );


        _piso.Draw(Core.SpriteBatch);
        _paredes.Draw(Core.SpriteBatch);
        Core.SpriteBatch.Draw(serpiente,_fum_posicion_mal,null,Color.White,0.0f,_fum_origen,1.0f,SpriteEffects.None,0.0f);
        if (isactive==true)
        {
            Core.SpriteBatch.Draw(fumigador,disparo,null,Color.White,rotar,_fum_origen,1.0f,SpriteEffects.None,0.0f);
            
        }
        else
        {
            Core.SpriteBatch.Draw(fumigador,Vector2.Zero,null,Color.White,rotar,_fum_origen,1.0f,SpriteEffects.None,0.0f);
        }
        Core.SpriteBatch.Draw(fumigador, _fum_posicion, null, Color.White, rotar, _fum_origen, 1.0f, SpriteEffects.None, 0.0f);

        Core.SpriteBatch.DrawString(
        font,
        "Casa de Prueba",
        new Vector2(15, 15),
        Color.Black,
          0f,
           Vector2.Zero, 2.0f,  
         SpriteEffects.None, 0f
        );
        Core.SpriteBatch.End();

        base.Draw(gameTime);

    }

}
