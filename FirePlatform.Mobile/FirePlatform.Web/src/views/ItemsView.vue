<template>
<div class="page">

    <TheNavbar></TheNavbar>
    <div class="row">
        <div class="col-sm-2">
        </div>
        <div class="col-sm-7">
            <div role="tablist" class="cardList">
                <h3 class="d-flex justify-content-center">Element list</h3>
                <b-card class="cardOverride" no-body v-for="(item, index) in mainCollection" v-bind:value="item.Value" :key="index">

                    <body>
                        <b-card-header class="d-flex justify-content-center p-1 cardHeaderOverride" header-tag="header" role="tab">
                            <b-btn class="blockOverride" block href="#" v-b-toggle="'collapse' + index" variant="info" v-model="item.name">{{item.title}}</b-btn>
                        </b-card-header>
                        <b-collapse :id="'collapse' + index" accordion="my-accordion" role="tabpanel">
                            <b-card-body>
                                <div class="form-group" :id="'collapse' + index" v-for="(itemDetails, index) in item.items" v-bind:value="itemDetails.Value" :key="index">
  <div v-if="itemDetails.isVisible === true">
   <div v-if="itemDetails.type === 'Text'" class="d-flex justify-content-center">
<label  class = "itemDetailsTitle" v-text = "itemDetails.title"/>
    <input  class = "itemDetailsValue form-control" :disabled="loading" type="text" 
    @change="recalculate(itemDetails.groupID, itemDetails.numID, itemDetails.value)" v-model="itemDetails.value" />
</div>
  <div v-else-if="itemDetails.type === 'Formula'" class="d-flex justify-content-center">    
    <label  class = "itemDetailsTitle" v-text = "itemDetails.title"/>
    <input class = "itemDetailsValue form-control" :disabled="true" type="text" 
    @change="recalculate(itemDetails.groupID, itemDetails.numID, itemDetails.value)" v-model="itemDetails.value" />
</div>
 <div v-else-if="itemDetails.type === 'BackCalcs'" class="d-flex justify-content-center">    
  <div v-if="backCalcsHidden === false">
    <label  class = "itemDetailsTitle" v-text = "itemDetails.title"/>
    <input class = "itemDetailsValue form-control" :disabled="loading" type="text" 
    @change="recalculate(itemDetails.groupID, itemDetails.numID, itemDetails.value)" v-model="itemDetails.value" />
</div>
</div>
<div v-else-if="itemDetails.type === 'Num'" class="d-flex justify-content-center">
  <label  class = "itemDetailsTitle" v-text = "itemDetails.title"/>
  <input class = "itemDetailsValue form-control" type="number" 
  @change="recalculate(itemDetails.groupID, itemDetails.numID, itemDetails.value)" v-model="itemDetails.value"/>
</div>
<div v-else-if="itemDetails.type === 'Check'" class="d-flex justify-content-center">
  <label  class = "itemDetailsTitle" v-text = "itemDetails.title"/>
  <div class="itemDetailsValue">
  <input class="itemDetailsValueCheckBox" :disabled="loading" type="checkbox"
   @change="recalculate(itemDetails.groupID, itemDetails.numID, itemDetails.value)" v-model="itemDetails.value"/>
  </div>
</div>
<div v-else-if="itemDetails.type === 'Combo'" class="d-flex justify-content-center">
     <label  class = "itemDetailsTitle" v-text = "itemDetails.title"/>
  <div class="itemDetailsValue">
      <select class="form-control" :disabled="loading" v-model="selectedItem"
      @change="recalculate(itemDetails.groupID, itemDetails.numID, selectedItem.comboItemTag.split(',')[0])"> 
        <option   
            v-for="(selectedItem, index) in (itemDetails ? itemDetails.multiItemDict : [])" 
            v-bind:value="selectedItem" :key="index">
            {{selectedItem.comboItemTitle}}
        </option>
      </select>
</div>
</div>
<div v-else-if="itemDetails.type === 5" class="d-flex justify-content-center">
   <label  class = "itemDetailsTitle" v-text = "itemDetails.title"/>
  <div class="itemDetailsValueMultiSelect">
      <select multiple class="form-control itemDetailsValueMultiSelectElement" :disabled="loading" size="60">
        <option v-for="(selectedItem, index) in itemDetails.value" 
         v-bind:value="selectedItem" :key="index">{{selectedItem}}</option>
        <option>{{selectedItem}}</option>
      </select>
</div>
</div>
<div v-else-if="itemDetails.type === 6" class="d-flex justify-content-center">
   <label  class = "itemDetailsTitle" v-text = "itemDetails.title"/>
  <div class="radio itemDetailsRadioButton">
      <form>
         <div class="radio" 
         v-for="(selectedItem, index) in itemDetails.value" 
         v-bind:value="selectedItem" :key="index">
            <label>
                <input type="radio" name="optradio" :disabled="loading"> {{selectedItem}}
                </label>
                                        </div>
                                        </form>
                                    </div>
                                </div>
            </div>
        </div>
        </b-card-body>
        </b-collapse>
        </body>
        </b-card>
    </div>
</div>
<div class="col-sm-3 cardList">
    <label  class = "itemOptionsTitle">Hide BackCalcs</label>
    <input class="itemDetailsValueCheckBox" :disabled="loading" type="checkbox"
        v-model="backCalcsHidden" @change="hideBackCalcs(backCalcsHidden)"/>
</div>
</div>
</div>
</template>

<script>
import axios from 'axios'
import TheNavbar from '@/components/TheNavbar.vue'
import {
    serverBus
} from '../main.js';

export default {
    name: 'ItemsView',
    components: {
        TheNavbar
    },
    data: () => ({
        mainCollection: [],
        loading: false,
        backCalcsHidden: false
    }),
    methods: {
        some(ev) {
            alert(ev);
        },
        recalculate: function (groupId, itemId, value) {

            this.$Progress.start();
            this.loading = true;

            axios.get('https://localhost:44358/api/Calculations/Set?groupId=' + groupId + '&itemId=' + itemId + '&value=' + value)
                .then(response => {

                    var items = response.data.item2;
                    items.forEach(newItem => {

                        var isExists = false;
                        this.mainCollection.forEach(itemGroup => {
                            itemGroup.items.forEach(item => {
                                if (item.numID === newItem.numID &&
                                    item.groupID === newItem.groupID) {

                                    item.groupID = newItem.groupID;
                                    item.groupTitle = newItem.groupTitle;
                                    item.numID = newItem.numID;
                                    item.title = newItem.title;
                                    item.type = newItem.type;
                                    item.isVisible = newItem.isVisible;
                                    item.tooltipText = newItem.tooltipText;
                                    item.value = newItem.value;

                                    isExists = true;
                                }
                            })
                        });

                        if (!isExists) {
                            this.mainCollection.forEach(itemGroup => {
                                if (itemGroup.indexGroup === newItem.groupID) {
                                    alert("group: " + newItem.groupID + " item: " + newItem.title + " value: " + newItem.value);
                                    itemGroup.items.push(newItem);
                                }
                            })
                        }
                    });

                    this.loading = false;
                    this.$Progress.finish();

                })
                .catch(e => {
                    this.loading = false;
                    this.$Progress.fail();

                    this.errors.push(e);
                })
        },
        hideBackCalcs(hidden) {
            if (hidden === true) {
                this.hideBackCalcs = "hidden";
            } else {
                this.hideBackCalcs = "none";
            }

        }
    },
    created() {
        serverBus.$on('itemsGroup', (mainCollection) => {
            this.mainCollection = mainCollection;
        })
    }
}
</script>

<style>
body {
    background-color: #e9ebee !important;
}

.itemOptionsTitle {
    height: 30px;
    width: 30%;
    padding: 10%;
}
</style>
