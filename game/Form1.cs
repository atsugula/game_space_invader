﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game
{
    public partial class Form1 : Form
    {

        // Movimiento del jugador
        bool goLeft, goRight;
        // Velocidad del jugador
        int playerSpeed = 12;
        // Velocidad de los enemigos
        int enemySpeed = 5;
        // Puntaje del jugador
        int score = 0;
        // cuando generar las balas enemigas
        int enemyBulletTimer = 300;

        // cuadro de imagenes de los invasores, es un array, para que sean varias
        PictureBox[] sadInvadersArray;

        bool shooting;
        bool isGameOver;

        public Form1()
        {
            InitializeComponent();
            gameSetup();
        }

        private void player_Click(object sender, EventArgs e)
        {

        }

        private void mainGameTimerEvent(object sender, EventArgs e)
        {

            txtScore.Text = $"Score: {score}";

            if (goLeft)
            {
                player.Left -= playerSpeed;
            }

            if (goRight)
            {
                player.Left += playerSpeed;
            }

            enemyBulletTimer -= 10;

            if (enemyBulletTimer < 1)
            {
                enemyBulletTimer = 300;
                makeBullet("sadBullet");
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "sadInvaders")
                {
                    
                    x.Left += enemySpeed;

                    if (x.Left > 730)
                    {
                        x.Top += 65;
                        x.Left = -80;
                    }

                    if (x.Bounds.IntersectsWith(player.Bounds))
                    {
                        gameOver("Has sido invadido por los invasores, ¡ahora estás triste!");
                    }

                    foreach (Control y in this.Controls)
                    {

                        if (y is PictureBox && (string)y.Tag == "bullet")
                        {
                            if (y.Bounds.IntersectsWith(x.Bounds))
                            {
                                this.Controls.Remove(x);
                                this.Controls.Remove(y);
                                score += 1;
                                shooting = false;
                            }
                        }

                    }

                }

                if (x is PictureBox && (string)x.Tag == "bullet")
                {
                    x.Top -= 20;

                    if (x.Top < 15)
                    {
                        this.Controls.Remove(x);
                        shooting = false;
                    }

                }

                // Si falla el tiro por aca cambiamos
                if (x is PictureBox && (string)x.Tag == "sadBullet")
                {
                    x.Top += 20;

                    if (x.Top > 620)
                    {
                        this.Controls.Remove(x);
                    }

                    if (x.Bounds.IntersectsWith(player.Bounds))
                    {
                        this.Controls.Remove(x);
                        gameOver("Te han matado. Ahora estarás triste para siempre.");
                    }

                }

            }

            if (score > 8)
            {
                enemySpeed = 12;
            }

            if (score == sadInvadersArray.Length)
            {
                gameOver("Woohoo Felicidades, ¡Mantenlo a salvo!");
            }


        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Space && shooting == false)
            {
                shooting = true;
                makeBullet("bullet");
            }
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeAll();
                gameSetup();
            }
        }
        
        private void makeInvaders()
        {
            sadInvadersArray = new PictureBox[15];

            int left = 0;

            for (int i = 0; i < sadInvadersArray.Length; i++)
            {

                sadInvadersArray[i] = new PictureBox();
                sadInvadersArray[i].Size = new Size(60, 50);
                sadInvadersArray[i].Image = Properties.Resources.alien;
                sadInvadersArray[i].Top = 0;
                sadInvadersArray[i].Tag = "sadInvaders";
                sadInvadersArray[i].Left = left;
                sadInvadersArray[i].SizeMode = PictureBoxSizeMode.StretchImage;
                this.Controls.Add(sadInvadersArray[i]);
                left = left - 80;

            }

        }

        // Funcion para definir o comenzar el juego
        private void gameSetup()
        {
            txtScore.Text = "Score: 0";
            score = 0;
            isGameOver = false;

            enemyBulletTimer = 300;
            enemySpeed = 5;
            shooting = false;

            makeInvaders();
            gameTimer.Start();
        }

        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();
            txtScore.Text = $"Score: {score} {message}";
        }

        private void removeAll()
        {

            foreach (PictureBox i in sadInvadersArray)
            {
                this.Controls.Remove(i);
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "bullet" || (string)x.Tag == "sadBullet")
                    {
                        this.Controls.Remove(x);
                    }
                }
            }

        }

        private void makeBullet(string bulletTag)
        {
            PictureBox bullet = new PictureBox();
            bullet.Image = Properties.Resources.bala_1;
            bullet.Size = new Size(24, 24);
            bullet.Tag = bulletTag;
            // Estara en la mitad de la imagen del jugador
            // Como si recreara un rayo o algo así
            bullet.Left = player.Left + player.Width / 2;

            if ((string)bullet.Tag == "bullet")
            {
                bullet.Top = player.Top - 20;
            }
            else if ((string)bullet.Tag == "sadBullet") {
                bullet.Top = - 100;
            }

            this.Controls.Add(bullet);

            // Poner la bala, por encima de todo
            bullet.BringToFront();

        }

    }
}
