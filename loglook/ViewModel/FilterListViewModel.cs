﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Autofac.Features.OwnedInstances;
using Model;

namespace ViewModel
{
    public class FilterListViewModel : ViewModelBase, IFilterListViewModel, IDisposable
    {
        private readonly IFileModel m_fileModel;
        private readonly Func<int, Owned<IFilterItemViewModel>> m_filterItemViewModelFactory;
        private readonly List<Owned<IFilterItemViewModel>> m_ownedFilterItems = new List<Owned<IFilterItemViewModel>>();

        public FilterListViewModel(IFileModel fileModel, Func<int, Owned<IFilterItemViewModel>> filterItemViewModelFactory)
        {
            m_fileModel = fileModel;
            m_filterItemViewModelFactory = filterItemViewModelFactory;
            AddSearchStringCommand = new RelayCommand(AddSearchString);
            AddSearchString("*All*");
        }

        public RelayCommand AddSearchStringCommand { get; }

        public void Dispose()
        {
            FilterItems.Clear();
            foreach (var ownedFilterItem in m_ownedFilterItems) ownedFilterItem.Dispose();
            m_ownedFilterItems.Clear();
        }

        public string Name => "FilterListViewModel";

        public ObservableCollection<IFilterItemViewModel> FilterItems { get; } =
            new ObservableCollection<IFilterItemViewModel>();


        private void AddSearchString(object reservedSearchStringIn)
        {
            var reservedSearchString = reservedSearchStringIn as string;
            var newFilterItem = m_filterItemViewModelFactory(m_ownedFilterItems.Count);
            if (!string.IsNullOrWhiteSpace(reservedSearchString))
            {
                if (reservedSearchString.Equals("*All*"))
                {
                    newFilterItem.Value.SearchString = reservedSearchString;
                    newFilterItem.Value.IsReservedSearchString = true;
                }
            }
            m_ownedFilterItems.Add(newFilterItem);
            FilterItems.Add(newFilterItem.Value);
        }
    }

    public interface IFilterListViewModel
    {
        string Name { get; }
        ObservableCollection<IFilterItemViewModel> FilterItems { get; }
    }
}