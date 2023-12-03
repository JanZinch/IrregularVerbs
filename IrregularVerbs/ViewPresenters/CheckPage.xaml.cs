﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using IrregularVerbs.Models;
using IrregularVerbs.Services;

namespace IrregularVerbs.ViewPresenters;

public partial class CheckPage : Page
{
    private ObservableCollection<IrregularVerbAnswer> _answers;
    private IrregularVerbsTeacher _teacher;
    
    public CheckPage()
    {
        InitializeComponent();
        _teacher = new IrregularVerbsTeacher(App.Instance.IrregularVerbsStorage, 12, false);
        _answers = new ObservableCollection<IrregularVerbAnswer>(_teacher.GenerateTask());
        Loaded += OnPageLoaded;
    }
    
    private void OnPageLoaded(object sender, RoutedEventArgs args)
    {
        _tableView.ItemsSource = _answers;
    }
    
    private void OnCheckButtonClick(object sender, RoutedEventArgs e)
    {
        _teacher.CheckTask();
    }
}