﻿using matthewsmith_c968.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace matthewsmith_c968
{
    public partial class MainForm : Form
    { 
        private static int updatedPartID = 9455;
        private int GeneratedID()
        {
            Random random = new Random();
            return random.Next(2423, updatedPartID);
        }

        public MainForm()
        {
            InitializeComponent();
            MainScreen();
         
            this.Text = "Main Screen";
            
          
        }
        public void MainScreen()
        {
            // Prepopulate the mock data for Parts and Products
            Inventory.mockData();

            // Set the data to the Parts Grid View
            var totalPartsInventory = new BindingSource();
            totalPartsInventory.DataSource = Inventory.Parts;
            partGrid.DataSource = totalPartsInventory;

        
            // Entire row selected
            partGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // Read only Grid
            partGrid.ReadOnly = true;
            // Cannot select more than one row
            partGrid.MultiSelect = false;
            // Cannot create rows
            partGrid.AllowUserToAddRows = false;

            // Set the data to the Products Grid View
            var totalProductsInventory = new BindingSource();
            totalProductsInventory.DataSource = Inventory.Products;
            productGrid.DataSource = totalProductsInventory;

            // Entire row selected
            productGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // Read only Grid
            productGrid.ReadOnly = true;
            // Cannot select more than one row
            productGrid.MultiSelect = false;
            // Cannot create rows
            productGrid.AllowUserToAddRows = false;

        }

        // Clears selected row on application start
        private void ClearBindingParts(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            partGrid.ClearSelection();
        }

        private void ClearBindingProducts(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            productGrid.ClearSelection();
        }

        // *************** Search Part Event ***************//
        private void SearchPart_Button(object sender, EventArgs e)
        {   
            string searchValue = searchPartInput.Text;
          
            if(!int.TryParse(searchValue, out _)){
                MessageBox.Show("Please enter a numeric value");
                return;
            }

            int searchID = int.Parse(searchPartInput.Text);
            Part matchingPartFound = Inventory.LookupPart(searchID);
            if(searchID < 0 )
            {
                MessageBox.Show("Please enter a numeric value greater than 0");
                return;
            }
            if (matchingPartFound == null)
            {
                MessageBox.Show("This Part ID does not excist");
                return;
            }
            foreach (DataGridViewRow currentIteration in partGrid.Rows)
            {
                Part part = (Part)currentIteration.DataBoundItem;
                if (part.PartID == matchingPartFound.PartID)
                {
                    currentIteration.Selected = true;
                    break;
                }
                else 
                {
                    currentIteration.Selected = false;
                }
            }

        }



        // *************** Part Click Events ***************//

        // Part Add 
        private void PartAdd_Button(object sender, EventArgs e)
        {
            new AddPartForm().ShowDialog();
        }

        // Part Modify
        private void PartModify_Button(object sender, EventArgs e)
        {
            Part part = partGrid.CurrentRow.DataBoundItem as Part;
            int generatedID = GeneratedID();

            if (partGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("No part was selected to modify", "Please make a selection!");
                return;
            }
            if (part is Inhouse inhouse)
            {
                ModifyPartForm form = new ModifyPartForm(generatedID, inhouse);
                form.ShowDialog();
            }
            if (part is Outsourced outsourced)
            {
                ModifyPartForm form = new ModifyPartForm(generatedID, outsourced);
                form.ShowDialog();

            }
        }
        // Part Delete
        private void PartDelete_Button(object sender, EventArgs e)
        {

            if (partGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("No part was selected to delete", "Please make a selection!");
                return;
            }
            foreach (DataGridViewRow row in partGrid.SelectedRows)
                {
                    partGrid.Rows.RemoveAt(row.Index);

            } 
        }
        // *************** Search Part Event ***************//
        private void SearchProduct_Button(object sender, EventArgs e)
        {
            string searchValue = searchProductInput.Text;

            if (!int.TryParse(searchValue, out _))
            {
                MessageBox.Show("Please enter a numeric value");
                return;
            }

            int searchID = int.Parse(searchProductInput.Text);
            Product matchingPartFound = Inventory.LookupProduct(searchID);
            if (searchID < 0)
            {
                MessageBox.Show("Please enter a numeric value greater than 0");
                return;
            }
            if (matchingPartFound == null)
            {
                MessageBox.Show("This Part ID does not excist");
                return;
            }
            foreach (DataGridViewRow currentIteration in productGrid.Rows)
            {
                Product part = (Product)currentIteration.DataBoundItem;
                if (part.ProductID == matchingPartFound.ProductID)
                {
                    currentIteration.Selected = true;
                    break;
                }
                else
                {
                    currentIteration.Selected = false;
                }
            }
        }
        // *************** Product Click Events ***************//

        // Add Product
        private void ProductAdd_Button(object sender, EventArgs e)
        {
            new AddProductForm().ShowDialog();
        }
        // Modify Product
        private void ProductModify_Button(object sender, EventArgs e)
        {

        }

        // Delete Product
        private void ProductDelete_Button(object sender, EventArgs e)
        {
            if (productGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("No product was selected", "Please make a selection");
                return;
            }

            foreach (DataGridViewRow row in productGrid.SelectedRows)
            {
                productGrid.Rows.RemoveAt(row.Index);
            }

            if (productGrid.CurrentRow != null)
            {
                // Checks if the Product thats clicked is null or does NOT have Associated parts
                Product product = productGrid.CurrentRow.DataBoundItem as Product;

                if (product != null && product.AssociatedParts.Count > 0)
                {
                    MessageBox.Show("Please remove assigned Parts before deleting a Product");
                    return;
                }

            }
        
        }

     
    }
}
