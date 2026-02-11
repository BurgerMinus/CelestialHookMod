using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CelestialHookMod.Common;
using System.Collections.Generic;

namespace CelestialHookMod.Items
{

	internal class PhantasmalHook : ModItem
	{
		
		public override void SetDefaults() 
		{
			Item.CloneDefaults(ItemID.DualHook);
            Item.shootSpeed = 25f;
            Item.shoot = ModContent.ProjectileType<PhantasmalHookProjectile>(); 
		}

		public override void AddRecipes() 
		{
			
			CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 4)
                .AddIngredient(ItemID.DualHook)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
			
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            List<string> keys = KeybindSystem.PhantasmalHookRetract.GetAssignedKeys();
            string key = "N/A";
            if (keys.Count > 0)
            {
                key = keys[0];
            }
            TooltipLine newline = new TooltipLine(Mod, "PHookTutorial", "Press \'" + key + "\' to activate and recall hook");
            tooltips.Insert(2, newline);
        }
    }

	internal class PhantasmalHookProjectile : ModProjectile
	{
		private static Asset<Texture2D> chainTexture;
		private bool flag = false;
		private Player p = null;
		
		public override void Load() 
		{ 
			chainTexture = ModContent.Request<Texture2D>("CelestialHookMod/Items/PhantasmalHookChain");
		}

		public override void Unload() 
		{ 
			chainTexture = null;
		}

		public override void SetDefaults() 
		{
			Projectile.CloneDefaults(ProjectileID.DualHookRed);
            Projectile.width = 22;
            Projectile.height = 22;
        }

        public override bool? CanUseGrapple(Player player) 
		{
			
			int hooksOut = 0;
			for (int l = 0; l < 1000; l++) 
			{
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == Projectile.type) {
					hooksOut++;
				}
			}

			return hooksOut < 1;
		}

		public override float GrappleRange() 
		{
			return 500f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks) 
		{
			numHooks = 1;
		}

		public override void PostAI()
		{

			if (!flag && p != null)
			{
				flag = true;
				Projectile.velocity += p.velocity;
			}

            if (KeybindSystem.PhantasmalHookRetract.JustPressed)
            {
				RecallHook(false);
            }
        }

		public override void GrappleRetreatSpeed(Player player, ref float speed) 
		{
			speed = 35f;
			RecallHook(true);
		}

		private void RecallHook(bool playSound)
		{

            int dust;
            for (int i = 0; i < 12; i++)
            {
                dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Phantasmal, 0f, 0f, 100, default(Color), 1.2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 2f;
            }
            for (int i = 0; i < 8; i++)
            {
                dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BubbleBlock, 0f, 0f, 100, default(Color), 0.8f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 5f;
            }

			if (playSound)
			{
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
            }

            Projectile.Kill();
        }

		public override void GrapplePullSpeed(Player player, ref float speed) 
		{
			speed = 20f; 
		}

		public override bool? GrappleCanLatchOnTo(Player player, int x, int y) 
		{
            p = player; // store player reference to adjust projectile velocity

            int hooksOut = 0;
			for (int l = 0; l < 1000; l++) 
			{
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == Projectile.type) 
				{
					hooksOut++;
				}
			}
			
			if (KeybindSystem.PhantasmalHookRetract.JustPressed) 
			{
				if (hooksOut > 0) {return true;}
			}

			return false;
		}

		public override bool PreDrawExtras() 
		{
			Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
			Vector2 center = Projectile.Center;
			Vector2 directionToPlayer = playerCenter - Projectile.Center;
			float chainRotation = directionToPlayer.ToRotation() - MathHelper.PiOver2;
			float distanceToPlayer = directionToPlayer.Length();

			while (distanceToPlayer > 20f && !float.IsNaN(distanceToPlayer)) 
			{
				directionToPlayer /= distanceToPlayer; // get unit vector
				directionToPlayer *= chainTexture.Height(); // multiply by chain link length

				center += directionToPlayer; // update draw position
				directionToPlayer = playerCenter - center; // update distance
				distanceToPlayer = directionToPlayer.Length();

				Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));

				// Draw chain
				Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
					chainTexture.Value.Bounds, drawColor, chainRotation,
					chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
			}

            return false;
		}

        public override bool PreAI()
        {
            Lighting.AddLight(Projectile.Center, 0.7f, 0.7f, 0.7f);
            return true;
        }

    }

}
