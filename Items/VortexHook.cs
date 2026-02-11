using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialHookMod.Items
{

	internal class VortexHook : ModItem
	{
		public override void SetDefaults() 
		{
			Item.CloneDefaults(ItemID.LunarHook);
			Item.shootSpeed = 17.5f;
			Item.shoot = ModContent.ProjectileType<VortexHookProjectile>(); 
		}

		public override void AddRecipes() 
		{
			
			CreateRecipe()
				.AddIngredient(ItemID.FragmentVortex, 8)
				.AddIngredient(ItemID.AntiGravityHook)
				.AddTile(TileID.LunarCraftingStation)
				.Register();

		}
	}

	internal class VortexHookProjectile : ModProjectile
	{
		private static Asset<Texture2D> chainTexture;

		public override void Load() 
		{ 
			chainTexture = ModContent.Request<Texture2D>("CelestialHookMod/Items/VortexHookChain");
		}

		public override void Unload() 
		{ 
			chainTexture = null;
		}

		public override void SetDefaults() 
		{
			Projectile.CloneDefaults(ProjectileID.LunarHookVortex); 
			AIType = ProjectileID.AntiGravityHook;
            Projectile.width = 20;
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

			return hooksOut <= 6;
		}

		public override float GrappleRange() 
		{
			return 600f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks) 
		{
			numHooks = 6;
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed) 
		{
			speed = 24f; 
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
				directionToPlayer /= distanceToPlayer; 
				directionToPlayer *= chainTexture.Height(); 

				center += directionToPlayer; 
				directionToPlayer = playerCenter - center;
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
            Lighting.AddLight(Projectile.Center, 0.2f, 0.7f, 0.5f);
            return true;
        }

    }
}
