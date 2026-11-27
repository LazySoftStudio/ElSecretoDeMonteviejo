using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAPI.Core;
using BehaviourAPI.Core.Actions;
using BehaviourAPI.Core.Perceptions;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.UtilitySystems;

public class USAldeanoGenerico : BehaviourRunner
{
	[SerializeField] private AldeanoGenerico m_AldeanoGenerico;
	[SerializeField] private TrabajoBandos m_TrabajoBandos;
	private PushPerception newpushPerception;
	
	protected override void Init()
	{
		m_AldeanoGenerico = GetComponent<AldeanoGenerico>();
		m_TrabajoBandos = GetComponent<TrabajoBandos>();
		
		base.Init();
	}
	
	protected override BehaviourGraph CreateGraph()
	{
		UtilitySystem AldeanoGenericoUS = new UtilitySystem(1.3f);
		
		VariableFactor Cansancio = AldeanoGenericoUS.CreateVariable(m_AldeanoGenerico.IncrementarCansancio, 0f, 1f);
		
		LinearCurveFactor GanasDescansar = AldeanoGenericoUS.CreateCurve<LinearCurveFactor>(Cansancio);
		
		SimpleAction Descansar_action = new SimpleAction();
		Descansar_action.action = m_AldeanoGenerico.IrADescansar;
		UtilityAction Descansar = AldeanoGenericoUS.CreateAction(GanasDescansar, Descansar_action);
		
		VariableFactor GanasComer = AldeanoGenericoUS.CreateVariable(m_AldeanoGenerico.IncrementarGanasComer, 0f, 1f);
		
		SigmoidCurveFactor GanasComer_1 = AldeanoGenericoUS.CreateCurve<SigmoidCurveFactor>(GanasComer);
		
		SimpleAction Comer_action = new SimpleAction();
		Comer_action.action = m_AldeanoGenerico.IrAComer;
		UtilityAction Comer = AldeanoGenericoUS.CreateAction(GanasComer_1, Comer_action);
		
		VariableFactor GanasBanio = AldeanoGenericoUS.CreateVariable(m_AldeanoGenerico.IncrementarGanasBanio, 0f, 1f);
		
		SigmoidCurveFactor GanasIrBaño = AldeanoGenericoUS.CreateCurve<SigmoidCurveFactor>(GanasBanio);
		
		SimpleAction Banio_action = new SimpleAction();
		Banio_action.action = m_AldeanoGenerico.IrABanio;
		UtilityAction Banio = AldeanoGenericoUS.CreateAction(GanasIrBaño, Banio_action);
		
		WeightedFusionFactor Trabajar = AldeanoGenericoUS.CreateFusion<WeightedFusionFactor>(GanasDescansar, GanasComer_1, GanasIrBaño);
		
		LinearCurveFactor GanasTrabajar = AldeanoGenericoUS.CreateCurve<LinearCurveFactor>(Trabajar);
		
		SimpleAction Trabajar_1_action = new SimpleAction();
		Trabajar_1_action.action = m_TrabajoBandos.Pasear;
		UtilityAction Trabajar_1 = AldeanoGenericoUS.CreateAction(GanasTrabajar, Trabajar_1_action);
		
		newpushPerception = new PushPerception();
		return AldeanoGenericoUS;
	}
}
