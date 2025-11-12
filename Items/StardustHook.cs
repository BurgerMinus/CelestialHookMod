using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CelestialHookMod.Items
{
	internal class StardustHook : ModItem
	{
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.LunarHook);
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<StardustHookProjectile>(); // Makes the item shoot the hook's projectile when used.
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			
			CreateRecipe()
				.AddIngredient(ItemID.FragmentStardust, 8)
				.AddIngredient(ItemID.StaticHook)
				.AddTile(TileID.LunarCraftingStation)
				.Register();

		}
	}

	internal class StardustHookProjectile : ModProjectile
	{
		private static Asset<Texture2D> chainTexture;

		public override void Load() { // This is called once on mod (re)load when this piece of content is being loaded.
			// This is the path to the texture that we'll use for the hook's chain. Make sure to update it.
			chainTexture = ModContent.Request<Texture2D>("CelestialHookMod/Items/StardustHookChain");
		}

		public override void Unload() { // This is called once on mod reload when this piece of content is being unloaded.
			// It's currently pretty important to unload your static fields like this, to avoid having parts of your mod remain in memory when it's been unloaded.
			chainTexture = null;
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.StaticHook); 
			AIType = ProjectileID.StaticHook;
		}

		public override bool? CanUseGrapple(Player player) {
			int hooksOut = 0;
			for (int l = 0; l < 1000; l++) {
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == Projectile.type) {
					hooksOut++;
				}
			}

			return hooksOut <= 3;
		}

		public override float GrappleRange() {
			return 750f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks) {
			numHooks = 3; 
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed) {
			speed = 24f; 
		}

		public override bool PreDrawExtras() {
			Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
			Vector2 center = Projectile.Center;
			Vector2 directionToPlayer = playerCenter - Projectile.Center;
			float chainRotation = directionToPlayer.ToRotation() - MathHelper.PiOver2;
			float distanceToPlayer = directionToPlayer.Length();

			while (distanceToPlayer > 20f && !float.IsNaN(distanceToPlayer)) {
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
	}
}
