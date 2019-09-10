﻿using System;
using System.Collections.Generic;
using UnityEngine;
using KSP.Localization;


namespace KERBALISM
{


	public sealed class ProcessDevice : Device
	{
		public ProcessDevice(ProcessController process_ctrl)
		{
			this.process_ctrl = process_ctrl;
		}

		public override string Name()
		{
			return process_ctrl.title.ToLower();
		}

		public override uint Part()
		{
			return process_ctrl.part.flightID;
		}

		public override string Info()
		{
			return Lib.Color(process_ctrl.IsRunning(), Localizer.Format("#KERBALISM_Generic_RUNNING"), Lib.KColor.Green, Localizer.Format("#KERBALISM_Generic_STOPPED"), Lib.KColor.Yellow);
		}

		public override void Ctrl(bool value)
		{
			if (!process_ctrl.toggle) return;
			process_ctrl.SetRunning(value);
		}

		public override void Toggle()
		{
			Ctrl(!process_ctrl.IsRunning());
		}

		ProcessController process_ctrl;
	}


	public sealed class ProtoProcessDevice : Device
	{
		public ProtoProcessDevice(ProtoPartModuleSnapshot process_ctrl, ProcessController prefab, uint part_id)
		{
			this.process_ctrl = process_ctrl;
			this.prefab = prefab;
			this.part_id = part_id;
		}

		public override string Name()
		{
			return prefab.title.ToLower();
		}

		public override uint Part()
		{
			return part_id;
		}

		public override string Info()
		{
			return Lib.Color(Lib.Proto.GetBool(process_ctrl, "running"), Localizer.Format("#KERBALISM_Generic_RUNNING"), Lib.KColor.Green, Localizer.Format("#KERBALISM_Generic_STOPPED"), Lib.KColor.Yellow);
		}

		public override void Ctrl(bool value)
		{
			if (!prefab.toggle) return;
			Lib.Proto.Set(process_ctrl, "running", value);
			ProtoPartSnapshot part_prefab = FlightGlobals.FindProtoPartByID(part_id);

			double capacity = prefab.capacity;
			var res = part_prefab.resources.Find(k => k.resourceName == prefab.resource);
			res.amount = value ? capacity : 0.0;
		}

		public override void Toggle()
		{
			Ctrl(!Lib.Proto.GetBool(process_ctrl, "running"));
		}

		private readonly ProtoPartModuleSnapshot process_ctrl;
		private ProcessController prefab;
		private readonly uint part_id;
	}


} // KERBALISM



