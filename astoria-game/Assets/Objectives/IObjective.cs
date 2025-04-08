

using System;

public interface IObjective
{
	bool IsCompleted { get; }	
	public void Initialize(ObjectiveData data);
	public void UpdateObjective();
	public void ForceComplete();
	public event Action OnCompleted;
}