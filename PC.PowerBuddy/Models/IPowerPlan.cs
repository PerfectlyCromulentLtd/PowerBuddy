using System;

namespace PC.PowerBuddy.Models
{
	public interface IPowerPlan
	{
		string Description
		{
			get;
		}
		Guid Id
		{
			get;
		}
		bool IsActive
		{
			get;
		}
		string Name
		{
			get;
		}

		void Activate();
		void Dispose();
	}
}