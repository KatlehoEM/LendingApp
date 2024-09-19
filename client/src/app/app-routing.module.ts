import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { LoanOffersComponent } from './components/loan-offers/loan-offers.component';
import { LenderDashboardComponent } from './components/lender-dashboard/lender-dashboard.component';
import { BorrowerDashboardComponent } from './components/borrower-dashboard/borrower-dashboard.component';
import { CreateOfferComponent } from './components/create-offer/create-offer.component';
import { LoanapplicationComponent } from './components/loanapplication/loanapplication.component';

const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'login', component: LoginComponent},
  { path: 'register', component: RegisterComponent},
  { path: 'loan-offers', component: LoanOffersComponent},
  { path: 'lender-dashboard', component: LenderDashboardComponent},
  { path: 'borrower-dashboard', component: BorrowerDashboardComponent},
  { path: 'create-offer', component: CreateOfferComponent},
  { path: 'loanapplication/lender/:loanOfferId', component: LoanapplicationComponent},
  { path: '**', component: HomeComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
