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
    private sealed class Projectile
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; }

        public Projectile(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
        }
    }

    private const int s_cantidadAlacranes = 6;
    private const int s_tamanoProyectil = 8;
    private static readonly Vector2 s_posicionInicial = new(50, 80);
    private Vector2 _fum_posicion, _fum_origen;
    private AnimatedSprite _fumigadorSprite,_fumigadorSpritemovimientolateral,_fumigadorSpritemovimiento,alacranspritemovimento;
    private Texture2D _texturaProyectil;
    private float rotar;
    private bool isws=false;
    private bool isad=false;
    private TextureAtlas atlasalcran,atlas;

    private readonly List<Vector2> _alacranes = new();
    private readonly List<Projectile> _proyectiles = new();

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
    public override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }
     public override void LoadContent()
    {
         atlas = TextureAtlas.FromFile(Content, "images/Atlasfumigador-definition.xml");
         atlasalcran = TextureAtlas.FromFile(Content, "images/Atlasalacran-definition.xml");
        alacranspritemovimento = atlasalcran.CreateAnimatedSprite("alacran-movimiento");
        font = Content.Load<SpriteFont>("Font/FontPrincipal");
        _fumigadorSprite = atlas.CreateAnimatedSprite("fumigador-quieto");
        _fumigadorSpritemovimientolateral = atlas.CreateAnimatedSprite("fumigador-movimientocostado");
        _fumigadorSpritemovimiento = atlas.CreateAnimatedSprite("fumigador-movimiento");
        _texturaProyectil = new Texture2D(Core.GraphicsDevice, 1, 1);
        _texturaProyectil.SetData(new[] { Color.OrangeRed });
        _fum_posicion = s_posicionInicial;

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

        _alacranes.AddRange(GenerarPosicionesAlacranes());

        // TODO: use this.Content to load your game content here
    }
    public override void Update(GameTime gameTime)
{
    CheckKeyboardInput();
        _fumigadorSprite.Update(gameTime);
        _fumigadorSpritemovimientolateral.Update(gameTime);
        _fumigadorSpritemovimiento.Update(gameTime);
        ActualizarProyectiles(gameTime);
        for (int i = 0; i < _alacranes.Count; i++)
        {
            Vector2 direccion = _fum_posicion - _alacranes[i];
            if (direccion != Vector2.Zero)
            {
                direccion.Normalize();
            }

            Vector2 posicionAnterior = _alacranes[i];
            Vector2 movimiento = direccion * 1.0f;

            Vector2 nuevaPosicion = new(posicionAnterior.X + movimiento.X, posicionAnterior.Y);
            if (!ChocaConParedAlacran(nuevaPosicion))
            {
                _alacranes[i] = nuevaPosicion;
            }

            nuevaPosicion = new(_alacranes[i].X, posicionAnterior.Y + movimiento.Y);
            if (!ChocaConParedAlacran(nuevaPosicion))
            {
                _alacranes[i] = nuevaPosicion;
            }

            if (ChocaConAlacran(_fum_posicion, _alacranes[i]))
            {
                _fum_posicion = s_posicionInicial;
                Core.ChangeScene(new GameOverScene()); 
            }
        }

        if (_alacranes.Count == 0)
        {
            _alacranes.AddRange(GenerarPosicionesAlacranes());
        }

        alacranspritemovimento.Update(gameTime);

    base.Update(gameTime);
}
private void Disparar(Vector2 direccion)
{
    Vector2 posicion = _fum_posicion + direccion * (_fumAncho / 2f + s_tamanoProyectil / 2f);
    _proyectiles.Add(new Projectile(posicion, direccion));
}

private Rectangle ObtenerHitboxProyectil(Vector2 posicion)
{
    int mitad = s_tamanoProyectil / 2;
    return new Rectangle(
        (int)posicion.X - mitad,
        (int)posicion.Y - mitad,
        s_tamanoProyectil,
        s_tamanoProyectil
    );
}

private Rectangle ObtenerHitboxAlacran(Vector2 posicion)
{
    return new Rectangle(
        (int)posicion.X - _fumAncho / 2,
        (int)posicion.Y - _fumAlto / 2,
        _fumAncho,
        _fumAlto
    );
}

private void ActualizarProyectiles(GameTime gameTime)
{
    float distancia = 300f * (float)gameTime.ElapsedGameTime.TotalSeconds;

    for (int i = _proyectiles.Count - 1; i >= 0; i--)
    {
        Projectile proyectil = _proyectiles[i];
        proyectil.Position += proyectil.Direction * distancia;
        Rectangle hitboxProyectil = ObtenerHitboxProyectil(proyectil.Position);

        if (_colisiones.Exists(pared => pared.Intersects(hitboxProyectil)))
        {
            _proyectiles.RemoveAt(i);
            continue;
        }

        bool impacto = false;
        for (int j = _alacranes.Count - 1; j >= 0; j--)
        {
            if (hitboxProyectil.Intersects(ObtenerHitboxAlacran(_alacranes[j])))
            {
                _alacranes.RemoveAt(j);
                impacto = true;
                break;
            }
        }

        if (impacto)
        {
            _proyectiles.RemoveAt(i);
        }
    }
}
private List<Vector2> GenerarPosicionesAlacranes()
{
    List<Vector2> posicionesDisponibles = new();

    for (int fila = 0; fila < _paredes.Rows; fila++)
    {
        for (int columna = 0; columna < _paredes.Columns; columna++)
        {
            if (fila < 2 || fila >= _paredes.Rows - 2
                || columna < 2 || columna >= _paredes.Columns - 2)
            {
                continue;
            }

            Vector2 posicion = new(
                (columna + 0.5f) * _paredes.TileWidth,
                (fila + 0.5f) * _paredes.TileHeight
            );

            if (!ChocaConParedAlacran(posicion)
                && Vector2.Distance(posicion, s_posicionInicial) >= 96)
            {
                posicionesDisponibles.Add(posicion);
            }
        }
    }

    for (int i = posicionesDisponibles.Count - 1; i > 0; i--)
    {
        int indiceAleatorio = Random.Shared.Next(i + 1);
        (posicionesDisponibles[i], posicionesDisponibles[indiceAleatorio]) =
            (posicionesDisponibles[indiceAleatorio], posicionesDisponibles[i]);
    }

    List<Vector2> posicionesElegidas = new();
    foreach (Vector2 posicion in posicionesDisponibles)
    {
        bool demasiadoCerca = false;
        foreach (Vector2 elegida in posicionesElegidas)
        {
            if (Vector2.Distance(posicion, elegida) < 96)
            {
                demasiadoCerca = true;
                break;
            }
        }

        if (!demasiadoCerca)
        {
            posicionesElegidas.Add(posicion);
            if (posicionesElegidas.Count == s_cantidadAlacranes)
            {
                return posicionesElegidas;
            }
        }
    }

    throw new InvalidOperationException(
        $"No hay suficientes posiciones libres para colocar {s_cantidadAlacranes} alacranes."
    );
}
private bool ChocaConParedAlacran(Vector2 posicion)
{
    Rectangle enemigoRect = new Rectangle(
        (int)posicion.X - _fumAncho / 2,
        (int)posicion.Y - _fumAlto / 2, 
        _fumAncho, 
        _fumAlto
    );

    foreach (Rectangle pared in _colisiones)
    {
        if (enemigoRect.Intersects(pared))
            return true;
    }

    return false;
}
private bool ChocaConAlacran(Vector2 posicionJugador, Vector2 posicionAlacran)
{
    Rectangle jugador = new Rectangle(
        (int)posicionJugador.X - _fumAncho / 2,
        (int)posicionJugador.Y - _fumAlto / 2,
        _fumAncho,
        _fumAlto
    );
    Rectangle alacran = new Rectangle(
        (int)posicionAlacran.X - _fumAncho / 2,
        (int)posicionAlacran.Y - _fumAlto / 2,
        _fumAncho,
        _fumAlto
    );

    return jugador.Intersects(alacran);
}
    private void CheckKeyboardInput()
    {
        Vector2 posAnterior = _fum_posicion;
    // Get a reference to the keyboard inof
    KeyboardInfo keyboard = Core.Input.Keyboard;
    isws=false;
    isad=false;

    // If the escape key is pressed, return to the title screen.
    if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
    {
        Core.ChangeScene(new TitleScene());
    }

    if (keyboard.IsKeyDown(Keys.W)  )
    {
        _fum_posicion.Y = _fum_posicion.Y- 3;
        isws=true;
    }
        if (keyboard.WasKeyJustPressed(Keys.Up))
        {
            rotar = 0.0f;
            Disparar(-Vector2.UnitY);
        }

    // if the S or Down keys are down, move the slime down on the screen.
    if (keyboard.IsKeyDown(Keys.S)  )
    {
        _fum_posicion.Y = _fum_posicion.Y+ 3;
        isws=true;
    }
        if (keyboard.WasKeyJustPressed(Keys.Down))
        {
            rotar = 3.14f;
            Disparar(Vector2.UnitY);
        }

    // If the A or Left keys are down, move the slime left on the screen.
    if (keyboard.IsKeyDown(Keys.A)  )
    {
        _fum_posicion.X = _fum_posicion.X- 3;
        isad=true;
    }
        if (keyboard.WasKeyJustPressed(Keys.Left))
        {
             rotar = 3.14f * 1.5f;
            Disparar(-Vector2.UnitX);
        }

    // If the D or Right keys are down, move the slime right on the screen.
    if (keyboard.IsKeyDown(Keys.D)  )
    {
        _fum_posicion.X = _fum_posicion.X+ 3;
        isad=true;
    }
        if (keyboard.WasKeyJustPressed(Keys.Right))
        {
             rotar = 3.14f * 0.5f;
            Disparar(Vector2.UnitX);
        }
        if (ChocaConPared(_fum_posicion))
            _fum_posicion.Y = posAnterior.Y;

        if (ChocaConPared(_fum_posicion))
            _fum_posicion.X = posAnterior.X;
}
public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);
        _fum_origen = new Vector2(_fumigadorSprite.Width / 2.0f, _fumigadorSprite.Height / 2.0f);
        Core.SpriteBatch.Begin(
            samplerState: SamplerState.PointClamp
        );
        _fumigadorSprite.Rotation = rotar;
        _fumigadorSprite.Origin = _fum_origen;
        _fumigadorSpritemovimientolateral.Rotation = rotar;
        _fumigadorSpritemovimientolateral.Origin = _fum_origen;
        _fumigadorSpritemovimiento.Rotation = rotar;
        _fumigadorSpritemovimiento.Origin = _fum_origen;
        alacranspritemovimento.CenterOrigin();


        _piso.Draw(Core.SpriteBatch);
        _paredes.Draw(Core.SpriteBatch);
        foreach (Vector2 posicionAlacran in _alacranes)
        {
            alacranspritemovimento.Draw(Core.SpriteBatch, posicionAlacran);
        }
        foreach (Projectile proyectil in _proyectiles)
        {
            Core.SpriteBatch.Draw(_texturaProyectil, ObtenerHitboxProyectil(proyectil.Position), Color.White);
        }

        if (isws && (rotar == 3.14f * 1.5f || rotar == 3.14f * 0.5f))
        {
            _fumigadorSpritemovimientolateral.Draw(Core.SpriteBatch, _fum_posicion);
        }
        else if (isad && (rotar == 3.14f * 1.5f || rotar == 3.14f * 0.5f))
        {
            _fumigadorSpritemovimiento.Draw(Core.SpriteBatch, _fum_posicion);
        }
        else if (isad)
        {
            _fumigadorSpritemovimientolateral.Draw(Core.SpriteBatch, _fum_posicion);
        }
        else if (isws)
        {
            _fumigadorSpritemovimiento.Draw(Core.SpriteBatch, _fum_posicion);
        }
        else
        {
            _fumigadorSprite.Draw(Core.SpriteBatch, _fum_posicion);
        }

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

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _texturaProyectil?.Dispose();
        }

        base.Dispose(disposing);
    }
}
