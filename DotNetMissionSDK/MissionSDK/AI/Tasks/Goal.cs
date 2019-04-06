using System;

namespace DotNetMissionSDK.AI.Tasks
{
	public class Goal
	{
		public Task task			{ get; private set; }
		public float weight			{ get; private set; }


		public Goal(Task task, float weight)
		{
			this.task = task;
			this.weight = weight;

			task.GeneratePrerequisites();
		}
	}
}
