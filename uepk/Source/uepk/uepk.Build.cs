// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class uepk : ModuleRules
{
	public uepk(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] {
			"Core",
			"CoreUObject",
			"Engine",
			"InputCore",
			"EnhancedInput",
			"AIModule",
			"StateTreeModule",
			"GameplayStateTreeModule",
			"UMG",
			"Slate"
		});

		PrivateDependencyModuleNames.AddRange(new string[] { });

		PublicIncludePaths.AddRange(new string[] {
			"uepk",
			"uepk/Variant_Platforming",
			"uepk/Variant_Platforming/Animation",
			"uepk/Variant_Combat",
			"uepk/Variant_Combat/AI",
			"uepk/Variant_Combat/Animation",
			"uepk/Variant_Combat/Gameplay",
			"uepk/Variant_Combat/Interfaces",
			"uepk/Variant_Combat/UI",
			"uepk/Variant_SideScrolling",
			"uepk/Variant_SideScrolling/AI",
			"uepk/Variant_SideScrolling/Gameplay",
			"uepk/Variant_SideScrolling/Interfaces",
			"uepk/Variant_SideScrolling/UI"
		});

		// Uncomment if you are using Slate UI
		// PrivateDependencyModuleNames.AddRange(new string[] { "Slate", "SlateCore" });

		// Uncomment if you are using online features
		// PrivateDependencyModuleNames.Add("OnlineSubsystem");

		// To include OnlineSubsystemSteam, add it to the plugins section in your uproject file with the Enabled attribute set to true
	}
}
