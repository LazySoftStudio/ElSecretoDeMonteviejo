using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.StateMachines;
using BehaviourAPI.BehaviourTrees;

// ESTE COMPORTAMIENTO TODAVÍA NO ESTÁ FINALIZADO ----------------------------------------------------------------------------------------------------
public class FSMAldeanoBando : BehaviourRunner
{
	[SerializeField] private TrabajoBandos m_TrabajoBandos;
	[SerializeField] private Transform FinConflicto_perception_OtherTransform;
	[SerializeField] private Transform CercaEnemigo_perception_OtherTransform;
	[SerializeField] private Transform FinSocializacion_perception_OtherTransform;
	[SerializeField] private Transform CercaMismoBando_perception_OtherTransform;
	
	protected override void Init()
	{
		m_TrabajoBandos = GetComponent<TrabajoBandos>();
		
		base.Init();
	}
	
	protected override BehaviourGraph CreateGraph()
	{
		FSM FSMAldeanoBando = new FSM();
		BehaviourTree BTSocializando = new BehaviourTree();
		BehaviourTree BTEnConflicto = new BehaviourTree();
		
		SimpleAction Pasear_action = new SimpleAction();
		Pasear_action.action = m_TrabajoBandos.Pasear;
		State Pasear = FSMAldeanoBando.CreateState(Pasear_action);
		
		SubsystemAction Socializando_action = new SubsystemAction(BTSocializando);
		State Socializando = FSMAldeanoBando.CreateState(Socializando_action);
		
		SubsystemAction EnConflicto_action = new SubsystemAction(BTEnConflicto);
		State EnConflicto = FSMAldeanoBando.CreateState(EnConflicto_action);
		
		DistancePerception FinConflicto_perception = new DistancePerception();
		FinConflicto_perception.OtherTransform = FinConflicto_perception_OtherTransform;
		FinConflicto_perception.MaxDistance = 10f;
		StateTransition FinConflicto = FSMAldeanoBando.CreateTransition(Pasear, EnConflicto, FinConflicto_perception);
		
		DistancePerception CercaEnemigo_perception = new DistancePerception();
		CercaEnemigo_perception.OtherTransform = CercaEnemigo_perception_OtherTransform;
		CercaEnemigo_perception.MaxDistance = 10f;
		StateTransition CercaEnemigo = FSMAldeanoBando.CreateTransition(EnConflicto, Pasear, CercaEnemigo_perception);
		
		DistancePerception FinSocializacion_perception = new DistancePerception();
		FinSocializacion_perception.OtherTransform = FinSocializacion_perception_OtherTransform;
		FinSocializacion_perception.MaxDistance = 10f;
		StateTransition FinSocializacion = FSMAldeanoBando.CreateTransition(Socializando, Pasear, FinSocializacion_perception);
		
		DistancePerception CercaMismoBando_perception = new DistancePerception();
		CercaMismoBando_perception.OtherTransform = CercaMismoBando_perception_OtherTransform;
		CercaMismoBando_perception.MaxDistance = 10f;
		StateTransition CercaMismoBando = FSMAldeanoBando.CreateTransition(Pasear, Socializando, CercaMismoBando_perception);
		
		ConditionNode EnActividad = BTSocializando.CreateDecorator<ConditionNode>(null /* missing child */);
		
		LeafNode UnirseSocializacion = BTSocializando.CreateLeafNode(null /* missing action */);
		
		SequencerNode Unirse = BTSocializando.CreateComposite<SequencerNode>(false, EnActividad, UnirseSocializacion);
		Unirse.IsRandomized = false;
		
		LeafNode Saludar = BTSocializando.CreateLeafNode(null /* missing action */);
		
		ConditionNode SaludoCompletado = BTSocializando.CreateDecorator<ConditionNode>(null /* missing child */);
		
		LeafNode AcercarseAldeano = BTSocializando.CreateLeafNode(null /* missing action */);
		
		ConditionNode AmigoAlLado = BTSocializando.CreateDecorator<ConditionNode>(null /* missing child */);
		
		LeafNode Socializar = BTSocializando.CreateLeafNode(null /* missing action */);
		
		SequencerNode Comenzar = BTSocializando.CreateComposite<SequencerNode>(false, Saludar, SaludoCompletado, AcercarseAldeano, AmigoAlLado, Socializar);
		Comenzar.IsRandomized = false;
		
		SelectorNode Inicio = BTSocializando.CreateComposite<SelectorNode>(false, Unirse, Comenzar);
		Inicio.IsRandomized = false;
		
		ConditionNode EnActividad_1 = BTEnConflicto.CreateDecorator<ConditionNode>(null /* missing child */);
		
		LeafNode Unirse_2 = BTEnConflicto.CreateLeafNode(null /* missing action */);
		
		LeafNode Discutiendo = BTEnConflicto.CreateLeafNode(null /* missing action */);
		
		SequencerNode Unirse_1 = BTEnConflicto.CreateComposite<SequencerNode>(false, EnActividad_1, Unirse_2, Discutiendo);
		Unirse_1.IsRandomized = false;
		
		LeafNode Acercarse = BTEnConflicto.CreateLeafNode(null /* missing action */);
		
		LeafNode Discutir = BTEnConflicto.CreateLeafNode(null /* missing action */);
		
		LeafNode Discutiendo_1 = BTEnConflicto.CreateLeafNode(null /* missing action */);
		
		SequencerNode Comenzar_1 = BTEnConflicto.CreateComposite<SequencerNode>(false, Acercarse, Discutir, Discutiendo_1);
		Comenzar_1.IsRandomized = false;
		
		SelectorNode Inicio_1 = BTEnConflicto.CreateComposite<SelectorNode>(false, Unirse_1, Comenzar_1);
		Inicio_1.IsRandomized = false;
		
		return FSMAldeanoBando;
	}
}
