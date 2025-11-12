using CelestialHookMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CelestialHookMod.Common
{
	public class PhantasmalHookRecipe : ModSystem
	{
		public override void PostAddRecipes() 
		{
			for (int i = 0; i < Recipe.numRecipes; i++) 
			{
				Recipe recipe = Main.recipe[i];
				if (recipe.HasResult(ItemID.LunarHook))
				{
					recipe.AddCustomShimmerResult(ModContent.ItemType<Items.PhantasmalHook>());
				}
			}
		}
	}
}
