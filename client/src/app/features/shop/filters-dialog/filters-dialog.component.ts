import { Component, inject } from '@angular/core';
import { ShopService } from '../../../Core/services/shop.service';
import { MatDivider } from '@angular/material/divider';
import { MatListModule, MatSelectionList} from '@angular/material/list'
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filters-dialog',
  imports: [MatDivider, MatListModule, MatButtonModule, FormsModule],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent {
shopService = inject(ShopService)
private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
//** Injection token that can be used to access the data that was passed
//step 3(filter): access data from shop component
data = inject(MAT_DIALOG_DATA)
//step 4 (filter) put the data in the selecteds
selectedBrands:string[] = this.data.selectedBrands;
selectedTypes: string[] = this.data.selectedTypes;

//step 5 (filter) collect user selection and send by to shop
applyFilters(){
  this.dialogRef.close({
    selectedBrands : this.selectedBrands,
    selectedTypes: this.selectedTypes
  })
}
}
