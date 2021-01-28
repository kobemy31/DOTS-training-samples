﻿using UnityEngine;
using Unity.Entities;
using Unity.Core;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

public class AntLineOfSightSystem : SystemBase
{
	private EntityCommandBufferSystem bufferSystem;
	protected override void OnCreate()
	{
		bufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
	}
	
    protected override void OnUpdate()
    {
	    var ecb = bufferSystem.CreateCommandBuffer();

	    var ecbWriter = ecb.AsParallelWriter();
	    
		// test line of sight to food
		Entities.
			WithAll<AntPathing>().
			WithNone<AntLineOfSight>().
			WithNone<HasFood>().
			ForEach((Entity entity, in Translation translation) =>
			{
				if (translation.Value.y > TempFood.FOOD_LOS_Y)
				{
					float dx = TempFood.FOODPOSX - translation.Value.x;
					float dy = TempFood.FOODPOSY - translation.Value.y;
					float degs = Mathf.Rad2Deg * Mathf.Atan2(dx, dy);

					AntLineOfSight antLos = new AntLineOfSight() { DegreesToGoal = degs };
					ecbWriter.AddComponent<AntLineOfSight>(0,entity,antLos);
				}
			}).ScheduleParallel();

		// test line of sight to home
		Entities.
			WithAll<AntPathing>().
			WithAll<HasFood>().
			WithNone<AntLineOfSight>().
			ForEach((Entity entity, in Translation translation) =>
			{
				if (translation.Value.y < TempFood.HOME_LOS_Y)
				{
					float dx = 0 - translation.Value.x;
					float dy = 0 - translation.Value.y;
					float degs = Mathf.Rad2Deg * Mathf.Atan2(dx, dy);

					AntLineOfSight antLos = new AntLineOfSight() { DegreesToGoal = degs };
					ecbWriter.AddComponent<AntLineOfSight>(0,entity, antLos);
				}
			}).ScheduleParallel();

		bufferSystem.AddJobHandleForProducer(Dependency);
	}
}