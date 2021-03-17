import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable } from '@angular/material/table';
import { Router } from '@angular/router';
import { AnimalDto } from 'src/app/generated-models/animal-dto';
import { SpeciesEnum } from 'src/app/generated-models/species-enum';
import { AnimalsService } from 'src/services/animals.service';
import { AnimalDeleteComponent } from '../animal-delete/animal-delete.component';
import { AnimalListDataSource } from './animal-list-datasource';

@Component({
  selector: 'app-animal-list',
  templateUrl: './animal-list.component.html',
  styleUrls: ['./animal-list.component.css']
})
export class AnimalListComponent implements AfterViewInit, OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<AnimalDto>;
  dataSource: AnimalListDataSource;
  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['id', 'name', 'species', 'breed', 'location', 'actions'];

  constructor(private router: Router,
    private animalsService: AnimalsService,
    public dialog: MatDialog,
  ) {
  }

  ngOnInit(): void {
    this.initializeTable();
  }

  initializeTable() {
    this.animalsService.getAnimals().subscribe(x => {
      this.dataSource = new AnimalListDataSource(x);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.table.dataSource = this.dataSource;
    });
  }

  ngAfterViewInit(): void {

  }

  public convertToName(id: number) {
    for (var enumMember in SpeciesEnum) {
      if (parseInt(enumMember) === id) {
        return SpeciesEnum[id];
      }
    }
  }

  openDialog(id: number): void {
    const dialogRef = this.dialog.open(AnimalDeleteComponent, {
      width: '300px',
      data: { id: id }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.initializeTable();
    })
  }
}
