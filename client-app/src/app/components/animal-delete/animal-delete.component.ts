import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AnimalsService } from 'src/services/animals.service';

@Component({
  selector: 'app-animal-delete',
  templateUrl: './animal-delete.component.html',
  styleUrls: ['./animal-delete.component.css']
})
export class AnimalDeleteComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<AnimalDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data,
    private animalsService: AnimalsService) { }

  ngOnInit(): void {
  }

  cancel(): void {
    this.dialogRef.close();
  }

  delete(id: number) {
    this.animalsService.deleteAnimal(id).toPromise()
      .then(result => {
        console.log(result);
        this.dialogRef.close();
      },
        error => console.error(error)
      );
  }

}
