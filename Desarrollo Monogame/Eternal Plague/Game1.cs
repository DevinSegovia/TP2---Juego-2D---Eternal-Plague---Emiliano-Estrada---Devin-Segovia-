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

namespace Eternal_Plague;

public class Game1 : Core
{
    private Texture2D fumigador;
    private Vector2 _fum_posicion, _fum_origen;
    private float rotar;


    private List<Rectangle> _colisiones = new List<Rectangle>();

    private Tilemap _piso;

    private Tilemap _paredes;

    private int _fumAncho = 24;
    private int _fumAlto = 24;
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
        //Texto principal
        font = Content.Load<SpriteFont>("Font/FontPrincipal");

        fumigador = Content.Load<Texture2D>("images/Fumigador");
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





    protected override void Update(GameTime gameTime)
    {



        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        var kstate = Keyboard.GetState();


        Vector2 posAnterior = _fum_posicion;

        // Movimiento con WASD o flechas
        if (kstate.IsKeyDown(Keys.W))
        {
            _fum_posicion.Y = _fum_posicion.Y - 2;

        }
        if (kstate.IsKeyDown(Keys.S))
        {
            _fum_posicion.Y = _fum_posicion.Y + 2;
        }


        if (ChocaConPared(_fum_posicion))
            _fum_posicion.Y = posAnterior.Y;


        if (kstate.IsKeyDown(Keys.A))
        {
            _fum_posicion.X = _fum_posicion.X - 2;
        }
        if (kstate.IsKeyDown(Keys.D))
        {
            _fum_posicion.X = _fum_posicion.X + 2;
        }

        if (ChocaConPared(_fum_posicion))
            _fum_posicion.X = posAnterior.X;


        if (kstate.IsKeyDown(Keys.Up))
        {
            rotar = 0.0f;

        }
        if (kstate.IsKeyDown(Keys.Down))
        {
            rotar = 3.14f;
        }
        if (kstate.IsKeyDown(Keys.Left))
        {
            rotar = 3.14f * 1.5f;
        }
        if (kstate.IsKeyDown(Keys.Right))
        {
            rotar = 3.14f * 0.5f;
        }




        // TODO: Add your update logic here
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _fum_origen = new Vector2(fumigador.Width / 2.0f, fumigador.Height / 2.0f);

        SpriteBatch.Begin(
            samplerState: SamplerState.PointClamp
        );


        _piso.Draw(SpriteBatch);
        _paredes.Draw(SpriteBatch);

        SpriteBatch.Draw(fumigador, _fum_posicion, null, Color.White, rotar, _fum_origen, 1.0f, SpriteEffects.None, 0.0f);

        SpriteBatch.DrawString(
        font,
        "Eternal Plague texto prueba",
        new Vector2(20, 20),
        Color.Black
        //  0f,
        //   Vector2.Zero, 2.0f,  agrandan tamaño texto
        // SpriteEffects.None, 0f
        );
        SpriteBatch.End();

        base.Draw(gameTime);

    }


}
