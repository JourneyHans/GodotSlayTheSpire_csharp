using System;
using System.Linq;
using framework.events;
using framework.extension;
using Godot;
using Godot.Collections;

public partial class StatusHandler : GridContainer {
    public event Action<Status.EType> StatusesApplied;
    private const float StatusApplyInterval = 0.25f;
    
    [Export] public Node2D StatusOwner;

    public override void _Ready() {
        GuiInput += OnGuiInput;
    }

    public void ApplyStatusesByType(Status.EType type) {
        if (type == Status.EType.EventBased) {
            return;
        }

        Array<Status> statusQueue = GetAllStatuses().Filter(status => status.Type == type);
        if (statusQueue.IsNullOrEmpty()) {
            StatusesApplied?.Invoke(type);
            return;
        }

        var tween = CreateTween();
        foreach (Status status in statusQueue) {
            tween.TweenCallback(Callable.From(() => status.ApplyStatus(StatusOwner)));
            tween.TweenInterval(StatusApplyInterval);
        }

        tween.Finished += () => { StatusesApplied?.Invoke(type); };
    }

    public void AddStatus(Status status) {
        bool stackable = status.StackType != Status.EStackType.Node;

        if (!HasStatus(status.ID)) {
            // new status
            StatusUI newStatusUI = StatusUI.Instantiate();
            AddChild(newStatusUI);
            newStatusUI.Status = status;
            newStatusUI.Status.StatusApplied += OnStatusApplied;
            newStatusUI.Status.InitializeStatus(StatusOwner);
            return;
        }

        if (!status.CanExpire && !stackable) {
            // unique status
            return;
        }

        if (status.CanExpire && status.StackType == Status.EStackType.Duration) {
            GetStatus(status.ID).Duration += status.Duration;
            return;
        }

        if (status.StackType == Status.EStackType.Intensity) {
            GetStatus(status.ID).Stacks += status.Stacks;
        }
    }

    private bool HasStatus(string id) {
        return this.GetChildren<StatusUI>().Any(statusUI => statusUI.Status.ID == id);
    }

    private Status GetStatus(string id) {
        return this.GetChildren<StatusUI>().FirstOrDefault(statusUI => statusUI.Status.ID == id)?.Status;
    }

    private Array<Status> GetAllStatuses() {
        var statuses = new Array<Status>();
        foreach (StatusUI statusUI in GetChildren()) {
            statuses.Add(statusUI.Status);
        }

        return statuses;
    }

    private void OnStatusApplied(Status status) {
        if (status.CanExpire) {
            status.Duration -= 1;
        }
    }

    private void OnGuiInput(InputEvent inputEvent) {
        if (inputEvent.IsActionPressed(InputKey.LeftMouse)) {
            EventDispatcher.TriggerEvent(StatusView.Event.StatusTooltipRequested, GetAllStatuses());
        }
    }
}