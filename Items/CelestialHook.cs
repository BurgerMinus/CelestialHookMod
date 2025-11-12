using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CelestialHookMod.Common;

namespace CelestialHookMod.Items
{
	internal class CelestialHook : ModItem
	{
		
		private int mode = -1;
		
		public override void SetDefaults() 
		{
			
			mode = (mode + 1) % 5;
			
			switch(mode)
			{
				case 0:
					Item.CloneDefaults(ItemID.DualHook);
					Item.shootSpeed = 25f;
					Item.shoot = ModContent.ProjectileType<PhantasmalHookProjectile>(); 
					break;
				case 1:
					Item.CloneDefaults(ItemID.LunarHook);
					Item.shootSpeed = 20f;
					Item.shoot = ModContent.ProjectileType<SolarHookProjectile>(); 
					break;
				case 2:
					Item.CloneDefaults(ItemID.LunarHook);
					Item.shootSpeed = 22.5f;
					Item.shoot = ModContent.ProjectileType<NebulaHookProjectile>(); 
					break;
				case 3:
					Item.CloneDefaults(ItemID.LunarHook);
					Item.shootSpeed = 17.5f;
					Item.shoot = ModContent.ProjectileType<VortexHookProjectile>(); 
					break;
				case 4:
					Item.CloneDefaults(ItemID.LunarHook);
					Item.shootSpeed = 15f;
					Item.shoot = ModContent.ProjectileType<StardustHookProjectile>(); 
					break;
					
			}

		}

		public override void AddRecipes() 
		{
				
			CreateRecipe()
                .AddIngredient<Items.PhantasmalHook>()
				.AddIngredient<Items.SolarHook>()
				.AddIngredient<Items.NebulaHook>()
				.AddIngredient<Items.VortexHook>()
				.AddIngredient<Items.StardustHook>()
				.AddIngredient(ItemID.LunarOre)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
				
        }
		
		public override void UpdateInventory(Player player) 
		{
			if (KeybindSystem.CelestialHookSwap.JustPressed) 
			{
				SetDefaults();
			}
		}
		
	}
	
	/*
	internal class CelestialHookHandler : ModPlayer
	{
		
		public override void PostUpdate() 
		{
			
			if ((KeybindSystem.CelestialHookSwap.JustPressed) && (Player.miscEquips[4] is CelestialHook))
			{
				Player.miscEquips[4].SetDefaults();
			}
			
		}
		
	}
	*/
	
}

